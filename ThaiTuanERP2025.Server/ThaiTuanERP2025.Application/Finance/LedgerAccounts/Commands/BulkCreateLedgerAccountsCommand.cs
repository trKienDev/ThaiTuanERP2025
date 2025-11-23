using MediatR;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Shared.Extensions;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts.Commands
{
        public sealed record BulkCreateLedgerAccountsCommand (IReadOnlyList<LedgerAccountExcelRow> Rows ) : IRequest<Unit>;

        public sealed class BulkCreateLedgerAccountsCommandHandler : IRequestHandler<BulkCreateLedgerAccountsCommand, Unit>
        {
                private readonly IUnitOfWork _uow;
                public BulkCreateLedgerAccountsCommandHandler(IUnitOfWork uow)
                {
                        _uow = uow;
                }
                public async Task<Unit> Handle(BulkCreateLedgerAccountsCommand command, CancellationToken cancellationToken)
                {
                        if (command.Rows is null || command.Rows.Count == 0)
                                throw new BusinessRuleViolationException("File không có dữ liệu để import.");

                        // 1 ) Chuẩn hoá dữ liệu
                        var normalized = command.Rows.Select(x => new
                        {
                                Number = x.Number.Trim().ToUpperInvariant(),
                                Name = x.Name.Trim(),
                                BalanceType = ParseBalanceType(x.BalanceType),
                                ParentNumber = string.IsNullOrWhiteSpace(x.ParentNumber) ? null : x.ParentNumber.Trim().ToUpperInvariant(),
                                LedgerAccountTypeCode = string.IsNullOrWhiteSpace(x.LedgerAccountTypeCode) ? null : x.LedgerAccountTypeCode.Trim().ToLowerInvariant(),
                                Raw = x
                        }).ToList();

                        // 2 ) Kiểm tra duplicate Number trong file
                        var duplicateNumbersInFile = normalized.GroupBy(x => x.Number)
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key)
                                .ToList();

                        if (duplicateNumbersInFile.Any())
                                throw new BusinessRuleViolationException($"Trong file có mã Number bị trùng: {string.Join(", ", duplicateNumbersInFile)}");

                        // 3 ) Kiểm tra trùng DB
                        var numbers = normalized.Select(x => x.Number).ToList();

                        var existedNumbers = await _uow.LedgerAccounts.ListAsync(
                                q => q.Active().Where(la => numbers.Contains(la.Number)),
                                cancellationToken: cancellationToken
                        );

                        if (existedNumbers.Any())
                        {
                                var existed = existedNumbers.Select(x => x.Number);
                                throw new BusinessRuleViolationException($"Dữ liệu đã tồn tại trong DB: {string.Join(", ", existed)}");
                        }

                        // 4 ) Load toàn bộ LedgerAccount cha từ DB theo Number
                        var parentNumbers = normalized.Where(x => x.ParentNumber != null)
                                .Select(x => x.ParentNumber!)
                                .Distinct()
                                .ToList();

                        var parentAccountsFromDb = await _uow.LedgerAccounts.ListAsync(
                                q => q.Active().Where(x => parentNumbers.Contains(x.Number)),
                                cancellationToken: cancellationToken
                        );

                        var parentDbDict = parentAccountsFromDb.ToDictionary(x => x.Number, x => x);


                        // 5 ) Load LedgerAccountTypeId theo LedgerAccountTypeCode
                        var typeCodes = normalized.Where(x => !string.IsNullOrWhiteSpace(x.Raw.LedgerAccountTypeCode))
                                .Select(x => x.Raw.LedgerAccountTypeCode!.Trim().ToLowerInvariant())
                                .Distinct()
                                .ToList();

                        var ledgerAccountTypes = await _uow.LedgerAccountTypes.ListAsync(
                                q => q.Active().Where(t => typeCodes.Contains(t.Code)),
                                cancellationToken: cancellationToken
                        );

                        var typeDict = ledgerAccountTypes.ToDictionary(x => x.Code, x => x);

                        var missingTypeCodes = typeCodes.Except(typeDict.Keys).ToList();
                        if (missingTypeCodes.Any())
                                throw new BusinessRuleViolationException($"LedgerAccountTypeCode không tồn tại trong DB: {string.Join(", ", missingTypeCodes)}");

                        // 6 ) Xây dictionary cho các LedgerAccount sẽ tạo (để xử lý parent trong file
                        var createdDict = new Dictionary<string, LedgerAccount>();

                        // 7 ) Xử lý import theo thứ tự (root trước, con sau)
                        int remaining = normalized.Count;
                        while (createdDict.Count < normalized.Count)
                        {
                                bool createdInThisRound = false;
                                foreach (var item in normalized)
                                {
                                        if (createdDict.ContainsKey(item.Number)) continue; // đã tạo rồi
                                        LedgerAccount? parent = null;

                                        if (item.ParentNumber != null)
                                        {
                                                // 1. parent có trong DB?
                                                if (parentDbDict.TryGetValue(item.ParentNumber, out var parentDb))
                                                {
                                                        parent = parentDb;
                                                }
                                                // 2. parent nằm trong file (tạo ở vòng trước)
                                                else if (createdDict.TryGetValue(item.ParentNumber, out var parentCreated))
                                                {
                                                        parent = parentCreated;
                                                }
                                                else
                                                {
                                                        // Parent chưa tồn tại -> bỏ qua, xử lý ở vòng sau
                                                        continue;
                                                }
                                        }

                                        // 8 ) Tạo LedgerAccount 
                                        Guid? ledgerAccountTypeId = null;
                                        if (item.LedgerAccountTypeCode != null)
                                        {
                                                var code = item.LedgerAccountTypeCode;

                                                if (!typeDict.TryGetValue(code, out var typeEntity))
                                                {
                                                        throw new BusinessRuleViolationException(
                                                                $"LedgerAccountTypeCode '{item.Raw.LedgerAccountTypeCode}' không tồn tại."
                                                        );
                                                }

                                                ledgerAccountTypeId = typeEntity.Id;
                                        }

                                        var ledger = new LedgerAccount ( 
                                                item.Number,
                                                item.Name,
                                                item.BalanceType,
                                                item.Raw.Description,
                                                ledgerAccountTypeId,
                                                parent?.Id
                                        );

                                        // 9 ) Set Path + Level
                                        ledger.SetParent(parent);
                                        createdDict[item.Number] = ledger;
                                        createdInThisRound = true;
                                }

                                if (!createdInThisRound)
                                        throw new BusinessRuleViolationException("Không thể xác định cha cho một số dòng trong file. Vui lòng kiểm tra ParentNumber.");

                                // safety break
                                if (--remaining <= 0) break;

                        }

                        // 10 ) Add to DB
                        foreach (var ledger in createdDict.Values)
                        {
                                await _uow.LedgerAccounts.AddAsync(ledger, cancellationToken);
                        }

                        await _uow.SaveChangesAsync(cancellationToken);

                        return Unit.Value;
                }

                private LedgerAccountBalanceType ParseBalanceType(string raw)
                {
                        if (string.IsNullOrWhiteSpace(raw))
                                return LedgerAccountBalanceType.None;

                        raw = raw.Trim();

                        // Nếu trong enum tên là Debit, Credit, Both...
                        if (Enum.TryParse<LedgerAccountBalanceType>(raw, ignoreCase: true, out var result))
                                return result;

                        // Nếu trong file bạn dùng tiếng Việt, có thể mapping tay:
                        return raw.ToLowerInvariant() switch
                        {
                                "nợ" => LedgerAccountBalanceType.Debit,
                                "có" => LedgerAccountBalanceType.Credit,
                                "lưỡng tính" or "both" => LedgerAccountBalanceType.Both,
                                _ => throw new BusinessRuleViolationException($"Giá trị BalanceType không hợp lệ: '{raw}'")
                        };
                }
        }
}

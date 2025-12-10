using MediatR;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Commands
{
        public sealed record BulkCreateLedgerAccountTypesCommand(
                IReadOnlyList<LedgerAccountTypeExcelRow> Rows
        ) : IRequest<Unit>;

        public sealed class BulkCreateLedgerAccountTypesCommandHandler : IRequestHandler<BulkCreateLedgerAccountTypesCommand, Unit>
        {
                private readonly IUnitOfWork _uow;
                public BulkCreateLedgerAccountTypesCommandHandler(IUnitOfWork uow)
                {
                        _uow = uow;
                }

                public async Task<Unit> Handle(BulkCreateLedgerAccountTypesCommand command, CancellationToken cancellationToken)
                {
                        if (command.Rows == null || command.Rows.Count == 0)
                                throw new BusinessRuleViolationException("File không có dữ liệu để import.");

                        var normalized = command.Rows.Select(x => new
                        {
                                Code = x.Code.Trim().ToLowerInvariant(),
                                Name = x.Name.Trim(),
                                Raw = x
                        }).ToList();

                        var duplicateCodesInFile = normalized.GroupBy(x => x.Code)
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key)
                                .ToList();

                        if (duplicateCodesInFile.Any())
                                throw new BusinessRuleViolationException($"Trong file có mã loại tài khoản bị trùng: {string.Join(", ", duplicateCodesInFile)}");

                        var duplicateNamesInFile = normalized.GroupBy(x => x.Name)
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key)
                                .ToList();

                        if (duplicateNamesInFile.Any())
                                throw new BusinessRuleViolationException($"Trong file có tên loại tài khoản bị trùng: {string.Join(", ", duplicateNamesInFile)}");

                        foreach (var item in normalized)
                        {
                                var exists = await _uow.LedgerAccountTypes.ExistAsync(
                                        q => q.Code == item.Code || q.Name == item.Name,
                                        cancellationToken
                                );

                                if (exists)
                                        throw new BusinessRuleViolationException($"Loại tài khoản '{item.Raw.Code}' hoặc '{item.Raw.Name}' đã tồn tại trong hệ thống.");
                        }

                        var entities = normalized.Select(x =>
                                new LedgerAccountType(
                                        code: x.Raw.Code,
                                        name: x.Raw.Name,
                                        kind: Enum.Parse<LedgerAccountTypeKind>(x.Raw.Kind),
                                        description: x.Raw.Description
                                )
                        ).ToList();

                        await _uow.LedgerAccountTypes.AddRangeAsync(entities, cancellationToken);
                        await _uow.SaveChangesAsync(cancellationToken);

                        return Unit.Value;
                }
        }
}

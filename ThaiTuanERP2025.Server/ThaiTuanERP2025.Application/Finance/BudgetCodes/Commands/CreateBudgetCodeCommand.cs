using MediatR;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.BudgetCodes.Commands
{
	public sealed record CreateBudgetCodeCommand(
		string Code,
		string Name,
		Guid BudgetGroupId,
		Guid? CashoutCodeId
	) : IRequest<Unit>;

	public sealed class CreateBudgetCodeCommandHandler : IRequestHandler<CreateBudgetCodeCommand, Unit> {
		private readonly IUnitOfWork _uow;
		public CreateBudgetCodeCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(CreateBudgetCodeCommand command, CancellationToken cancellationToken) {
			var nameRaw = command.Name.Trim() ?? string.Empty;
			Guard.AgainstNullOrWhiteSpace(nameRaw, nameof(command.Name));	

			var codeRaw = command.Code.Trim() ?? string.Empty;
			Guard.AgainstNullOrWhiteSpace(codeRaw, nameof(command.Code));

			Guard.AgainstNullOrEmptyGuid(command.BudgetGroupId, nameof(command.BudgetGroupId));
			var groupExisted = await _uow.BudgetGroups.ExistAsync(q => q.Id == command.BudgetGroupId, cancellationToken);
			if (!groupExisted) throw new NotFoundException("Không tìm thấy nhóm ngân sách yêu cầu");

			var codeNorm = codeRaw.ToUpperInvariant();
			var nameNorm = nameRaw;

			var existed = await _uow.BudgetCodes.ExistAsync(
				q => q.Code == codeNorm || q.Name.ToLower() == nameNorm.ToLower()!,
				cancellationToken
			);
			if (existed) throw new ConflictException("Mã ngân sách đã tồn tại");

			var newCode = new BudgetCode(codeNorm, nameNorm, command.BudgetGroupId);


			if (command.CashoutCodeId.HasValue)
			{
				var cashoutId = command.CashoutCodeId.Value;
				Guard.AgainstNullOrEmptyGuid(cashoutId, nameof(command.CashoutCodeId));

				var cashoutExisted = await _uow.CashoutCodes.ExistAsync(q => q.Id == command.CashoutCodeId, cancellationToken);
				if (!cashoutExisted) throw new NotFoundException("Không tìm thấy mã dòng tiền ra yêu cầu");

				newCode.SetCashoutCode(cashoutId);
			}

			await _uow.BudgetCodes.AddAsync(newCode, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}

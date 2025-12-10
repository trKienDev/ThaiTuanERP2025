using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.CashoutCodes.Commands
{
	public sealed record CreateCashoutCodeCommand(CashoutCodePayload Payload) : IRequest<Unit>;

	public sealed class CreateCashoutCodeCommandHandler : IRequestHandler<CreateCashoutCodeCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public CreateCashoutCodeCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(CreateCashoutCodeCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;

			var normalizedName = payload.Name.Trim();

			var exist = await _uow.CashoutCodes.ExistAsync(
				x => x.IsActive && x.Name == normalizedName,
				cancellationToken: cancellationToken
			);
			if (exist) throw new BusinessRuleViolationException("Khoản iền ra này đã tồn tại");

			var cashoutGroup = await _uow.CashoutGroups.ExistAsync(
				x => x.IsActive && x.Id == payload.GroupId,
				cancellationToken: cancellationToken
			);
			if (!cashoutGroup) throw new NotFoundException("Nhóm khoản chi không tồn tại");

			var ledgerAccount = await _uow.LedgerAccounts.ExistAsync(
				x => x.IsActive && x.Id == payload.LedgerAccountId, 
				cancellationToken: cancellationToken
			);
			if (!ledgerAccount) throw new NotFoundException("Tài khoản hạch toán không tồn tại");

			var entity = new CashoutCode(payload.Name, payload.GroupId, payload.LedgerAccountId, payload?.Description);
			await _uow.CashoutCodes.AddAsync(entity, cancellationToken);

			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}

	public sealed class CreateCashoutCodeCommandValidator : AbstractValidator<CreateCashoutCodeCommand>
	{
		public CreateCashoutCodeCommandValidator()
		{
			RuleFor(x => x.Payload.Name).NotEmpty().WithMessage("Tên khoản chi không được để trống");
			RuleFor(x => x.Payload.GroupId).NotEmpty().WithMessage("Phải chọn nhóm khoản chi");
			RuleFor(x => x.Payload.LedgerAccountId).NotEmpty().WithMessage("Phải chọn tài khoản hạch toán");
		}
	}
}

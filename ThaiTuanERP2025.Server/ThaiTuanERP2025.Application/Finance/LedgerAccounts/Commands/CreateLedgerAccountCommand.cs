using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared.Extensions;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts.Commands
{
	public sealed record CreateLedgerAccountCommand(LedgerAccountPayload Payload) : IRequest<Unit>;

	public sealed class CreateLedgerAccountCommandHandler : IRequestHandler<CreateLedgerAccountCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public CreateLedgerAccountCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(CreateLedgerAccountCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;
			LedgerAccount? parent = null;

			var normalizedName = payload.Name.Trim();

			var exist = await _uow.LedgerAccounts.ExistAsync(
				x => x.Number == payload.Number || (x.Number == payload.Number && x.Name == normalizedName),
				cancellationToken: cancellationToken
			);
			if( exist ) throw new BusinessRuleViolationException("Tài khoản hạch toán này đã tồn tại");

			if (payload.ParentLedgerAccountId.HasValue) 
			{
				parent = await _uow.LedgerAccounts.SingleOrDefaultAsync(
					q => q.Active().Where(x => x.Id == payload.ParentLedgerAccountId.Value),
					cancellationToken: cancellationToken
				) ?? throw new NotFoundException("Không tìm thấy tài khoản cha");
			}

			var ledgerAccount = new LedgerAccount(
				payload.Number,
				payload.Name,
				payload.BalanceType,
				payload.Description,
				payload?.LedgerAccountTypeId,
				parent?.Id
			);

			if (parent is null)
			{
				ledgerAccount.SetParent(null); // Node root
			}
			else
			{
				ledgerAccount.SetParent(parent); // Node con
			}

			await _uow.LedgerAccounts.AddAsync(ledgerAccount, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}

	public class CreateLedgerAccountCommandValidator : AbstractValidator<CreateLedgerAccountCommand>
	{
		public CreateLedgerAccountCommandValidator()
		{
			RuleFor(x => x.Payload.Number)
			    .NotEmpty().WithMessage("Mã tài khoản là bắt buộc.");

			RuleFor(x => x.Payload.Name)
			    .NotEmpty().WithMessage("Tên tài khoản là bắt buộc.");
		}
	}
}

using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Contracts;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Commands
{
	public sealed record CreateOutgoingBankAccountCommand(OutgoingBankAccountPayload Payload) : IRequest<Unit>;
	public sealed class CreateOutgoignBankAccountCommandHandler : IRequestHandler<CreateOutgoingBankAccountCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public CreateOutgoignBankAccountCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(CreateOutgoingBankAccountCommand query, CancellationToken cancellationToken)
		{
			var payload = query.Payload;
			
			var nameNorm = payload.Name.Trim();
			var accountNumberNorm = payload.AccountNumber.Trim();
			var bankNameNorm = payload.BankName.Trim();
			var ownerNameNorm = payload.OwnerName.Trim();

			var exist = await _uow.OutgoingBankAccounts.ExistAsync(
				q => q.Name == nameNorm, cancellationToken
			);
			if (exist) throw new DomainException($"Tài khoản tiền ra {nameNorm} này đã tồn tại");

			var entity = new OutgoingBankAccount(nameNorm, bankNameNorm, accountNumberNorm, ownerNameNorm);
			await _uow.OutgoingBankAccounts.AddAsync(entity, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		} 
	}

	public sealed class CreateOutgoingBankAccountCommandValidator : AbstractValidator<CreateOutgoingBankAccountCommand>
	{
		public CreateOutgoingBankAccountCommandValidator()
		{
			RuleFor(x => x.Payload.Name)
				.NotEmpty().WithMessage("Tên tài khoản không được để trống")
				.MaximumLength(200).WithMessage("Tên tài khoản không vượt quá 200 ký tự");

			RuleFor(x => x.Payload.BankName)
				.NotEmpty().WithMessage("Tên ngân hàng không được để trống")
				.MaximumLength(200).WithMessage("Tên ngân hàng không vượt quá 200 ký tự");

			RuleFor(x => x.Payload.AccountNumber)
				.NotEmpty().WithMessage("Số tài khoản ngân hàng không được để trống")
				.MaximumLength(200).WithMessage("Số tài khoản ngân hàng không vượt quá 200 ký tự");

			RuleFor(x => x.Payload.OwnerName)
				.NotEmpty().WithMessage("Tên chủ tài khoản không được để trống")
				.MaximumLength(200).WithMessage("Tên chủ tài khoản không vượt quá 200 ký tự");
		}
	}
}

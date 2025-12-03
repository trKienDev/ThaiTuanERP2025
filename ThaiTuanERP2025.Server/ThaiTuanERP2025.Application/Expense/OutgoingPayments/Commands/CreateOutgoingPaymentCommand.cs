using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Application.Shared.Services;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Commands
{
	public sealed record CreateOutgoingPaymentCommand(OutgoingPaymentPayload Payload) : IRequest<Unit>;
	public sealed class CreateOutgoingPaymentCommandHandler : IRequestHandler<CreateOutgoingPaymentCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly IDocumentSubIdGeneratorService _documentSubIdGenerator;
		public CreateOutgoingPaymentCommandHandler(
			IUnitOfWork uow, IDocumentSubIdGeneratorService documentSubIdGenerator)
		{
			_uow = uow;
			_documentSubIdGenerator = documentSubIdGenerator;
		}

		public async Task<Unit> Handle(CreateOutgoingPaymentCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;

			// Normialization
			var nameNorm = payload.Name.Trim();
			var bankNameNorm = payload.BankName.Trim();
			var accountNumberNorm = payload.AccountNumber.Trim();
			var beneficiaryNameNorm = payload.BeneficiaryName.Trim();

			var exist = await _uow.OutgoingPayments.ExistAsync(
				q => q.Name == nameNorm, cancellationToken
			);
			if (exist) throw new ValidationException($"Khoản chi {nameNorm} đã tồn tại");

			var newOutgoingPayment = new OutgoingPayment(
				nameNorm,
				payload.OutgoingAmount,
				bankNameNorm,
				accountNumberNorm,
				beneficiaryNameNorm,
				payload.DueAt,
				payload.OutgoingBankAccountId,
				payload.ExpensePaymentId,
				payload.Description
			);

			var subId = await _documentSubIdGenerator.NextSubIdAsync(Domain.Shared.Enums.DocumentType.OutgoingPayment, DateTime.UtcNow, cancellationToken);
			newOutgoingPayment.SetSubId(subId);

			await _uow.OutgoingPayments.AddAsync(newOutgoingPayment, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}

	public sealed class CreateOutgoingPaymentCommandValidator : AbstractValidator<CreateOutgoingPaymentCommand>
	{
		public CreateOutgoingPaymentCommandValidator()
		{
			RuleFor(x => x.Payload.Name)
				.NotEmpty().WithMessage("Tên khoản chi không được để trống.")
				.MaximumLength(256).WithMessage("Tên khoản chi không được vượt quá 256 ký tự");

			RuleFor(x => x.Payload.OutgoingAmount)
				.NotEmpty().WithMessage("Số tiền khoản chi không được phép để trống")
				.GreaterThan(1000).WithMessage("Số tiền khoản chi không nhỏ hơn 1.000");

			RuleFor(x => x.Payload.BankName)
				.NotEmpty().WithMessage("Tên ngân hàng thụ hưởngh không được để trống.")
				.MaximumLength(256).WithMessage("Tên ngân hàng thụ hưởng không được vượt quá 128 ký tự");

			RuleFor(x => x.Payload.AccountNumber)
				.NotEmpty().WithMessage("Số tài khoản thụ hưởngh không được để trống.")
				.MaximumLength(256).WithMessage("Số tài khoản thụ hưởng không được vượt quá 64 ký tự");

			RuleFor(x => x.Payload.BeneficiaryName)
				.NotEmpty().WithMessage("Tên người thụ hưởngh không được để trống.")
				.MaximumLength(256).WithMessage("Tên người thụ hưởng không được vượt quá 256 ký tự");

			RuleFor(x => x.Payload.DueAt)
				.NotEmpty().WithMessage("Hạn thanh toán không được để trống")
				.Must(dueAt => dueAt.Date >= DateTime.Today).WithMessage("Hạn thanh toán không được phép nhỏ hơn ngày hôm nay"); ;

			RuleFor(x => x.Payload.OutgoingBankAccountId)
				.NotEmpty().WithMessage("Định danh tài khoản tiền ra không được trống");

			RuleFor(x => x.Payload.ExpensePaymentId)
				.NotEmpty().WithMessage("Định danh thanh toán không được hợp lệ");

			RuleFor(x => x.Payload.Description)
				.MaximumLength(2048).WithMessage("Mô tả không vượt quá 2048 ký tự");
			
		}
	}

}
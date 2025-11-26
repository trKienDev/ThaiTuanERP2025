using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Services;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Commands
{
	public sealed record CreateExpensePaymentCommand(ExpensePaymentPayload Payload) : IRequest<Unit>;
	
	public sealed class CreateExpensePaymentCommandHandler : IRequestHandler<CreateExpensePaymentCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly IDocumentSubIdGeneratorService _documentSubIdGeneratorService;
		public CreateExpensePaymentCommandHandler(
			IUnitOfWork uow, IDocumentSubIdGeneratorService documentSubIdGeneratorService
		) {
			_uow = uow; 
			_documentSubIdGeneratorService = documentSubIdGeneratorService;
		}

		public async Task<Unit> Handle(CreateExpensePaymentCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;

			// chuẩn hóa dữ liệu
			var nameNorm = payload.Name.Trim();
			var accountNumberNorm = payload.AccountNumber.Trim();
			var bankNameNorm = payload.BankName.Trim();
			var beneficiaryNameNorm = payload.BeneficiaryName.Trim();

			// payment
			var newPayment = new ExpensePayment(nameNorm, payload.hasGoodsReceipt, payload.PayeeType, payload.DueDate, payload.ManagerApproverId, payload.Description);

			// subId
			var subId = await _documentSubIdGeneratorService.NextSubIdAsync(DocumentType.ExpensePayment, DateTime.UtcNow, cancellationToken);
			newPayment.SetSubId(subId);

			// Validate Supplier
			var supplierExist = await _uow.Suppliers.ExistAsync(
				q => q.Id == payload.SupplierId && q.IsActive,
				cancellationToken: cancellationToken
			);
			if (!supplierExist) throw new NotFoundException("Không tìm thấy nhà cung cấp");
			newPayment.SetSupplier(payload.SupplierId);

			// Bank
			newPayment.SetBankInfo(bankNameNorm, accountNumberNorm, beneficiaryNameNorm);

			// Items
			foreach(var item in payload.Items)
			{
				newPayment.AddItem(
					itemName: item.ItemName.Trim(),
					quantity: item.Quantity,
					unitPrice: item.UnitPrice,
					taxRate: item.taxRate,
					totalWithTax: item.TotalWithTax,
					budgetPlanDetailId: item.BudgetPlanDetailId,
					invoiceFileId: item.InvoiceStoredFileId
				);
			}

			// Save
			await _uow.ExpensePayments.AddAsync(newPayment, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	} 

	public sealed class CreateExpensePaymentCommandValidator : AbstractValidator<CreateExpensePaymentCommand>
	{
		public CreateExpensePaymentCommandValidator()
		{
			RuleFor(x => x.Payload.Name)
				.NotEmpty().WithMessage("Tên chi phí không được để trống")
				.MaximumLength(256).WithMessage("Tên chi phí không vượt quá 256 ký tự");

			RuleFor(x => x.Payload.Description).MaximumLength(2048).WithMessage("Mô tả không vượt quá 2048 ký tự");
			RuleFor(x => x.Payload.PayeeType).NotEmpty().WithMessage("Đối tượng thụ hưởng không được để trống");
			RuleFor(x => x.Payload.BankName).NotEmpty().WithMessage("Tên ngân hàng không được để trống");
			RuleFor(x => x.Payload.DueDate).Must(d => d.Date >= DateTime.Today).WithMessage("Hạn thanh toán không được phép là ngày trong quá khứ");
			RuleFor(x => x.Payload.AccountNumber).NotEmpty().WithMessage("Số tài khoản ngân hàng không được để trống");
			RuleFor(x => x.Payload.BeneficiaryName).NotEmpty().WithMessage("Tên người thụ hưởng không được để trống");
			RuleFor(x => x.Payload.DueDate).NotEmpty().WithMessage("Hạn thanh toán không được để trống");
			RuleFor(x => x.Payload.ManagerApproverId).NotEmpty().WithMessage("Phải chọn user quản lý phê duyệt");
			RuleFor(x => x.Payload.Items).NotEmpty().WithMessage("Phiếu thanh toán phải có ít nhất 1 hạng mục chi");
			RuleForEach(x => x.Payload.Items).ChildRules(item =>
			{
				item.RuleFor(i => i.ItemName)
				    .NotEmpty().WithMessage("Tên hạng mục không được trống");

				item.RuleFor(i => i.Quantity)
				    .GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0");

				item.RuleFor(i => i.UnitPrice)
				    .GreaterThanOrEqualTo(0).WithMessage("Đơn giá không hợp lệ");
			});
		}
	}
}

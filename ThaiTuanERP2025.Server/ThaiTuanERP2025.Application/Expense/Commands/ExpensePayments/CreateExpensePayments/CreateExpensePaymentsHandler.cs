using MediatR;
using System.Net.WebSockets;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePayments
{
	public sealed class CreateExpensePaymentsHandler : IRequestHandler<CreateExpensePaymentCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ApprovalWorkflowService _approvalWorkflowService;
		private readonly IDocumentSubIdGeneratorService _documentSubIdGeneratorService;
		public CreateExpensePaymentsHandler(
			IUnitOfWork unitOfWork, 
			ApprovalWorkflowService approvalWorkflowService,
			IDocumentSubIdGeneratorService documentSubIdGeneratorService
		)
		{
			_unitOfWork = unitOfWork;
			_approvalWorkflowService = approvalWorkflowService;
			_documentSubIdGeneratorService = documentSubIdGeneratorService;
		}

		public async Task<Guid> Handle(CreateExpensePaymentCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;
			var subId = await _documentSubIdGeneratorService.NextSubIdAsync("ExpensePayment" ,DateTime.UtcNow, cancellationToken);
			// payment
			var payment = new ExpensePayment(request.Name, request.PayeeType, request.PaymentDate, request.ManagerApproverId, request.Description);
			payment.SetSubId(subId);
			payment.SetSupplier(request.PayeeType == PayeeType.Supplier ? request.SupplierId : null);
			payment.SetBankInfo(request.BankName, request.AccountNumber, request.BeneficiaryName);
			payment.SetGoodsReceipt(request.HasGoodsReceipt);

			// items
			foreach(var item in request.Items)
			{
				payment.AddItem(
					itemName: item.ItemName,
					quantity: item.Quantity,
					unitPrice: item.UnitPrice,
					taxRate: item.TaxRate,
					budgetCodeId: item.BudgetCodeId,
					cashoutCodeId: item.CashoutCodeId,
					invoiceId: item.InvoiceId,
					overrideTaxAmount: item.TaxAmount
				);
			}

			// followers
			if(request.FollowerIds?.Count > 0)
				payment.ReplaceFollowers(request.FollowerIds);

			// attachments
			foreach(var attachment in request.Attachments)
			{
				payment.AddAttachment(attachment.ObjectKey, attachment.FileName, attachment.Size, attachment.Url, attachment.FileId);
			}

			await _unitOfWork.ExpensePayments.AddAsync(payment);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			await _approvalWorkflowService.CreateInstanceForExpensePaymentAsync(
				paymentId: payment.Id,
				workflowTemplateId: Guid.Parse("77B4F0E4-91DE-4377-9AC3-C2A8A69EDF6E"),
				overrides: null,
				autoStart: false,
				linkToPayment: true,
				cancellationToken: cancellationToken
			);

			return payment.Id;
		}
	}
}

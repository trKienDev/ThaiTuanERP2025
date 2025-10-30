using AutoMapper.Execution;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Application.Followers.Services;
using ThaiTuanERP2025.Domain.Common.Enums;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Followers.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePayments
{
	public sealed class CreateExpensePaymentsHandler : IRequestHandler<CreateExpensePaymentCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IApprovalWorkflowService _approvalWorkflowService;
		private readonly IDocumentSubIdGeneratorService _documentSubIdGeneratorService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IFollowerService _followerService;
		public CreateExpensePaymentsHandler(
			IUnitOfWork unitOfWork,
			IApprovalWorkflowService approvalWorkflowService,
			IDocumentSubIdGeneratorService documentSubIdGeneratorService,
			ICurrentUserService currentUserService,
			IFollowerService followerService
		)
		{
			_unitOfWork = unitOfWork;
			_approvalWorkflowService = approvalWorkflowService;
			_documentSubIdGeneratorService = documentSubIdGeneratorService;
			_currentUserService = currentUserService;
			_followerService = followerService;
		}

		public async Task<Guid> Handle(CreateExpensePaymentCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;
			var subId = await _documentSubIdGeneratorService.NextSubIdAsync(DocumentType.ExpensePayment, DateTime.UtcNow, cancellationToken);
			// payment
			var payment = new ExpensePayment(request.Name, request.PayeeType, request.DueDate, request.ManagerApproverId, request.Description);
			payment.SetSubId(subId);
			payment.SetSupplier(request.PayeeType == PayeeType.Supplier ? request.SupplierId : null);
			payment.SetBankInfo(request.BankName, request.AccountNumber, request.BeneficiaryName);
			payment.SetGoodsReceipt(request.HasGoodsReceipt);

			// 1 ) gom các budgetCodeId trong request
			var budgetIds = request.Items.Select(i => i.BudgetCodeId).Distinct().ToList();
			var budgetCodes = await _unitOfWork.BudgetCodes.FindAsync(bc => budgetIds.Contains(bc.Id), cancellationToken);
			var bcToCashout = budgetCodes.ToDictionary(x => x.Id, x => x.CashoutCodeId);
			// items
			foreach (var item in request.Items)
			{
				payment.AddItem(
					itemName: item.ItemName,
					quantity: item.Quantity,
					unitPrice: item.UnitPrice,
					taxRate: item.TaxRate,
					budgetCodeId: item.BudgetCodeId,
					cashoutCodeId: bcToCashout[item.BudgetCodeId],
					invoiceId: item.InvoiceId,
					overrideTaxAmount: item.TaxAmount
				);
			}

			// followers
			if (request.FollowerIds?.Count > 0)
			{
				foreach (var uid in request.FollowerIds)
				{
					await _followerService.FollowAsync(SubjectType.ExpensePayment, payment.Id, uid, cancellationToken);
				}
			}

			// attachments
			foreach (var attachment in request.Attachments)
			{
				payment.AddAttachment(attachment.ObjectKey, attachment.FileName, attachment.Size, attachment.Url, attachment.FileId);
			}

			// trừ ngân sách
			var now = DateTime.UtcNow;
			var year = now.Year;
			var month = now.Month;
			var budgetPeriod = await _unitOfWork.BudgetPeriods.SingleOrDefaultIncludingAsync(
				p => p.Year == year && p.Month == month,
				cancellationToken: cancellationToken
			);
			if (budgetPeriod is null || !budgetPeriod.IsActive) 
				throw new ConflictException($"Kỳ ngân sách {month}/{year} chưa được khởi tạo.");

			var currentUserId = _currentUserService.UserId;
			var currentUser = await _unitOfWork.Users.SingleOrDefaultIncludingAsync(
				u => u.Id == currentUserId, cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy thông tin user");
			if (currentUser.DepartmentId is null)
				throw new ConflictException("User chưa được gán phòng ban.");
			var departmentId = currentUser.DepartmentId.Value;

			// Gom tổng tiền theo BudgetCodeId
			var byBudget = request.Items.GroupBy(i => i.BudgetCodeId)
				.Select(g => new {
					BudgetCodeId = g.Key,
					TotalWithTax = g.Sum(x => x.TotalWithTax)
				}).ToList();

			// Lấy các BudgetPlan tương ứng một lần
			var plans = await _unitOfWork.BudgetPlans.ListAsync(
				q => q.Where(
					bp => bp.DepartmentId == departmentId &&
					bp.BudgetPeriodId == budgetPeriod.Id && 
					budgetIds.Contains(bp.BudgetCodeId)
				),
				cancellationToken: cancellationToken
			);
			var planMap = plans.ToDictionary(bp => bp.BudgetCodeId);

			// Kiểm tra đủ ngân sách & trừ
			foreach (var grp in byBudget)
			{
				if (!planMap.TryGetValue(grp.BudgetCodeId, out var plan))
					throw new ConflictException($"Không tìm thấy BudgetPlan cho BudgetCodeId={grp.BudgetCodeId} Dept={departmentId} Period={month}/{year}.");

				if (!plan.CanAfford(grp.TotalWithTax))
					throw new ConflictException(
					    $"Ngân sách không đủ cho BudgetCodeId={grp.BudgetCodeId}. Còn lại {plan.Amount:n0}, cần {grp.TotalWithTax:n0}."
					);

				plan.RecordPayment(grp.TotalWithTax, payment.Id); // TRỪ TRỰC TIẾP THEO YÊU CẦU
				// nếu muốn an toàn concurrency: thêm RowVersion vào BudgetPlan và bật ConcurrencyCheck
			}

			await _unitOfWork.ExpensePayments.AddAsync(payment, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			await _approvalWorkflowService.CreateInstanceForExpensePaymentAsync(
				expensePayment: payment,
				workflowTemplateId: Guid.Parse("77B4F0E4-91DE-4377-9AC3-C2A8A69EDF6E"),
				overrides: null,
				linkToPayment: true,
				cancellationToken: cancellationToken
			);

			return payment.Id;
		}
	}
}

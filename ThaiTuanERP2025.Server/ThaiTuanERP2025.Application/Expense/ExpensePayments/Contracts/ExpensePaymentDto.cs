using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts
{
	public sealed record ExpensePaymentDto
	{

	}

	public sealed record ExpensePaymentLookupDto
	{
		public Guid Id { get; init; } = Guid.Empty;
		public string Name { get; init; } = string.Empty;	 
		public ExpensePaymentStatus Status { get; init; }
		public decimal TotalAmount { get; init; }
		public decimal TotalTax { get; init; }
		public decimal TotalWithTax { get; init; }
		public decimal OutgoingAmountPaid { get; init; }
		public decimal RemainingOutgoingAmount { get; init; }
		public DateTime CreatedAt { get; init; }

		public Guid CreatedByUserId { get; init; }
		public UserBriefAvatarDto CreatedByUser { get; init; } = new UserBriefAvatarDto();

		public ExpenseWorkflowInstanceBriefDto? WorkflowInstance { get; init; }
	}
}

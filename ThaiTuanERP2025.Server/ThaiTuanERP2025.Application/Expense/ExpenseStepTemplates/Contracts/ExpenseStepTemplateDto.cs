using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates.Contracts
{
	public sealed record ExpenseStepTemplateDto
	{
		public Guid WorkflowTemplateId { get; init; } = Guid.Empty;
		public string Name { get; init; } = string.Empty;
		public int Order { get; init; } = default!;
		public ExpenseFlowType FlowType { get; init; }
		public int SlaHours { get; init; } = default!;
		public ExpenseApproveMode ExpenseApproveMode { get; init; }
		public string? FixedApproverIdsJson { get; init; }

		// Chứa tên rule để xác định người duyệt động (creator-manager, "amount-based")
		public string? ResolverKey { get; init; }
		public string? ResolverParamsJson { get; init; }
	}
}

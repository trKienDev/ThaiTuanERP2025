using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates.Contracts
{
	public sealed record ExpenseStepTemplatePayload {
		public string Name { get; init; } = string.Empty;
		public int Order { get; init; } = 0;
		public string FlowType { get; init; } = ExpenseFlowType.Single.ToString();
		public int SlaHours { get; init; } = 8;
		public string ApproveMode { get; init; } = ExpenseApproveMode.Standard.ToString();

		// Standard
		public List<Guid>? ApproverIds { get; init; } = new List<Guid>();

		// Condition
		public string? ResolverKey { get; init; } = string.Empty;
		public object? ResolverParams { get; init; }
	};
}

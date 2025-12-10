namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts
{
	public sealed record ExpenseStepTemplateDto
	{
		public Guid WorkflowTemplateId { get; init; } = Guid.Empty;
		public string Name { get; init; } = string.Empty;
		public int Order { get; init; } = default!;
		public string FlowType { get; init; }
		public int SlaHours { get; init; } = default!;
		public string ApproveMode { get; init; } = string.Empty;
                public List<Guid> ApproverIds { get; set; } = new();

                // Chứa tên rule để xác định người duyệt động (creator-manager, "amount-based")
                public string? ResolverKey { get; init; }
		public string? ResolverParamsJson { get; init; }
	}
}

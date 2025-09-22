namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class ApprovalStepTemplateDto
	{
		public Guid Id { get; init; }
		public Guid WorkflowTemplateId { get; init; }
		public string Name { get; init; } = string.Empty;
		public int Order { get; init; }
		public string FlowType{ get; init; } = "Single";
		public int SlaHours { get; init; }
		public string ApproverMode { get; init; } = "Standard";

		// Standard
		public Guid[]? ApproverIds { get; init; }

		// Condition
		public string? ResolverKey { get; init; } // e.g. "creator-department-manager"
		public object? ResolverParams { get; init; } // arbitrary JSON (deserialize as needed)
		public bool AllowOverride { get; init; }	
	}

	public sealed record ApprovalStepTemplateRequest(
		Guid WorkflowTemplateId,
		string Name,
		int Order,
		string FlowType,
		int SlaHours,
		string ApproverMode,

		// Standard
		Guid[]? ApproverIds,

		// Condition
		string? ResolverKey,
		object? ResolverParams,
		bool AllowOverride
	);
}

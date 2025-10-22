namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ApprovalWorkflowInstanceDto
	{
		public Guid Id { get; init; }
		public Guid TemplateId { get; init; }
		public int TemplateVersion { get; init; }
		public string DocumentType { get; init; } = string.Empty;
		public Guid DocumentId { get; init; }
		public Guid CreatedByUserId { get; init; }
		public DateTime CreatedAt { get; init; }
		public string Status { get; init; } = "Draft";
		public int? CurrentStepOrder { get; init; }

		public decimal? Amount { get; init; }
		public string? Currency { get; init; }
		public string? BudgetCode { get; init; }
		public string? CostCenter { get; init; }

		public IReadOnlyList<ApprovalStepInstanceDto> Steps { get; init; } = Array.Empty<ApprovalStepInstanceDto>();
	};

	public sealed class ApprovalWorkflowInstanceRequest
	{
		public Guid TemplateId { get; init; }
		public Guid DocumentId { get; init; }
		public string DocumentType { get; init; } = string.Empty;
		public Guid CreatorId { get; init; }

		// Context for resolvers & denormalized dimensions
		public decimal? Amount { get; init; }
		public string? Currency { get; init; }
		public string? BudgetCode { get; init; }
		public string? CostCenter { get; init; }

		// Optional: allow client to request immediate start (activate first step)
		public bool StartImmediately { get; init; } = true;
	};

	public sealed record ApprovalWorkflowInstanceDetailDto {
		public ApprovalWorkflowInstanceDto WorkflowInstance { get; init; } = default!;
		public int CurrentStepOrder { get; init; } = default!;
		public IReadOnlyList<ApprovalStepInstanceDetailDto> Steps { get; init; } = Array.Empty<ApprovalStepInstanceDetailDto>();
	}

	public sealed record ApprovalWorkflowInstanceStatusDto {
		public string Status { get; init; } = default!;
		public int CurrentStepOrder { get; init; } = default!;
		public IReadOnlyList<ApprovalStepInstanceStatusDto> Steps { get; init; } = Array.Empty<ApprovalStepInstanceStatusDto>();
	};
}

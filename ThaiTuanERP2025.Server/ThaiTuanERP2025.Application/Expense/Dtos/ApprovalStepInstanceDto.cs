using ThaiTuanERP2025.Application.Account.Users;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public record ApprovalStepInstanceDto(
		Guid Id,
		Guid WorkflowInstanceId,
		Guid? TemplateStepId,
		string Name,
		int Order,
		string FlowType,
		int SlaHours,
		string ApprovalMode,

		Guid[]? ResolvedApproverCandidateIds,
		Guid? DefaultApproverId,
		Guid? SelectedApproverId,

		string Status,
		DateTime? StartedAt,
		DateTime? DueAt,
		DateTime? ApprovedAt,
		Guid? ApprovedBy,
		DateTime? RejectedAt,
		Guid? RejectedBy,

		string? Comments,
		bool SlaBreached,
		object? History
	);

	public record ApprovalStepInstanceDetailDto {
		public Guid Id { get; init; }
		public Guid WorkflowInstanceId { get; init; }
		public Guid? TemplateStepId { get; init; }
		public string Name { get; init; } = default!;
		public int Order { get; init; }
		public string FlowType { get; init; } = default!;
		public int SlaHours { get; init; }
		public string ApprovalMode { get; init; } = default!;

		public Guid[]? ResolvedApproverCandidateIds { get; init; }
		public UserDto[]? ApproverCandidates { get; init; }
		public Guid? DefaultApproverId { get; init; }
		public UserDto DefaultApproverUser { get; init; } = default!;
		public Guid? SelectedApproverId { get; init; }

		public string Status { get; init; } = default!;
		public DateTime? StartedAt { get; init; }
		public DateTime? DueAt { get; init; }

		public DateTime? ApprovedAt { get; init; }
		public Guid? ApprovedBy { get; init; }
		public UserDto? ApprovedByUser { get; init; }

		public DateTime? RejectedAt { get; init; }
		public Guid? RejectedBy { get; init; }
		public UserDto? RejectedByUser { get; init; }

		public string? Comments { get; init; }
		public bool SlaBreached { get; init; }
		public object? History { get; init; }
	}

	public record ApproveStepRequest
	{
		public Guid UserId { get; set; }
		public Guid PaymentId { get; set; }
		public string? Comment { get; set; }
	};
	public sealed record RejectStepRequest : ApproveStepRequest;

	public sealed record ApprovalStepInstanceStatusDto
	{
		public string Status { get; init; } = default!;   // enum → string
		public DateTime? StartedAt { get; init; }
		public DateTime? DueAt { get; init; }

		public Guid DefaultApproverId { get; init; }
		public UserDto DefaultApproverUser { get; init; } = default!;

		public DateTime? ApprovedAt { get; init; }
		public Guid? ApprovedBy { get; init; }
		public UserDto? ApprovedByUser { get; init; }

		public DateTime? RejectedAt { get; init; }
		public Guid? RejectedBy { get; init; }
		public UserDto? RejectedByUser { get; init; }

		public int Order { get; init; }
	}
}

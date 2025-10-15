using ThaiTuanERP2025.Application.Account.Dtos;

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

	public record ApprovalStepInstanceDetailDto(
		Guid Id,
		Guid WorkflowInstanceId,
		Guid? TemplateStepId,
		string Name,
		int Order,
		string FlowType,
		int SlaHours,
		string ApprovalMode,

		Guid[]? ResolvedApproverCandidateIds,
		UserDto[]? ApproverCandidates,
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
	) : ApprovalStepInstanceDto(
		Id,
		WorkflowInstanceId,
		TemplateStepId,
		Name,
		Order,
		FlowType,
		SlaHours,
		ApprovalMode,
		ResolvedApproverCandidateIds,
		DefaultApproverId,
		SelectedApproverId,
		Status,
		StartedAt,
		DueAt,
		ApprovedAt,
		ApprovedBy,
		RejectedAt,
		RejectedBy,
		Comments,
		SlaBreached,
		History
	);

	public record ApproveStepRequest
	{
		public Guid UserId { get; set; }
		public Guid PaymentId { get; set; }
		public string? Comment { get; set; }
	};
	public sealed record RejectStepRequest : ApproveStepRequest;

	public sealed record ApprovalStepInstanceStatusDto(
		string Status,
		DateTime? StartedAt,
		DateTime? DueAt,
		DateTime? ApprovedAt,
		Guid? ApprovedBy,
		DateTime? RejectedAt,
		Guid? RejectedBy
	);
}

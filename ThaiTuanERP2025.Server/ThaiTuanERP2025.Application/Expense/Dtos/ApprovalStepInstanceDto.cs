namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ApprovalStepInstanceDto(
		Guid Id,
		Guid WorkflowInstanceId,
		Guid? TemplateStepId,
		string Name,	
		int Order,
		string FlowType,
		int SlaHours,
		string ApprovalMode,

		Guid[]? ResolvedApproverCandidates,
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
}

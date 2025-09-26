namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ApprovalWorkflowTemplateDto
	(
		Guid Id,
		string Name,
		string DocumentType,    
		int Version,
		bool IsActive
	);

	public sealed record class ApprovalWorkflowTemplateRequest
	(
		string Name,
		string DocumentType,
		int Version,
		bool IsActive
	);

	public sealed class ApprovalWorkflowTemplateDetailDto
	{
		public ApprovalWorkflowTemplateDto Template { get; init; } = default!;
		public IReadOnlyList<ApprovalStepTemplateDto> Steps { get; init; } = Array.Empty<ApprovalStepTemplateDto>();
	}
}


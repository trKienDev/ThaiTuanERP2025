namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public record ApprovalWorkflowDto
	{
		public string Name { get; init; } = string.Empty;
		public List<ApprovalStepDto> Steps { get; init; } = new List<ApprovalStepDto>();
	}
}

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts
{
	public sealed record ExpenseWorkflowTemplateDto
	{
		public Guid Id { get; init; } = Guid.Empty;
		public string Name { get; init; } = string.Empty;
		public int Version { get; init; }
		public bool IsActive { get; init; }
		public ICollection<ExpenseStepTemplateDto> Steps { get; set; } = new List<ExpenseStepTemplateDto>();
	}
}

using ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates.Contracts;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts
{
	public sealed record ExpenseWorkflowTemplateDto
	{
		public Guid Id { get; init; } = Guid.Empty;
		public string Name { get; init; } = string.Empty;
		public int Version { get; init; }
		public bool IsActive { get; init; }
		public ICollection<ExpenseStepTemplateDto> Steps { get; init; } = new List<ExpenseStepTemplateDto>();
	}
}

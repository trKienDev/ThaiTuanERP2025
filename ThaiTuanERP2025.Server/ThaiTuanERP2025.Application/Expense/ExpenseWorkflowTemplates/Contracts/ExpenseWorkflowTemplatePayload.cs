using ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates.Contracts;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts
{
	public sealed record ExpenseWorkflowTemplatePayload
	{
		public string Name { get; init; } = string.Empty;
		public int Version { get; init; } = 1;
		
		public List<ExpenseStepTemplatePayload> Steps { get; init; } = new List<ExpenseStepTemplatePayload>();
	}
}

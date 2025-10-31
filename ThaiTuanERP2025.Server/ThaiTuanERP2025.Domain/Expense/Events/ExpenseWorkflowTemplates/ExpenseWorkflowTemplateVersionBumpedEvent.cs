using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpenseWorkflowTemplates
{
	public sealed class ExpenseWorkflowTemplateVersionBumpedEvent : IDomainEvent
	{
		public ExpenseWorkflowTemplateVersionBumpedEvent(ExpenseWorkflowTemplate workflowTemplate, int newVersion)
		{
			WorkflowTemplate = workflowTemplate;
			NewVersion = newVersion;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseWorkflowTemplate WorkflowTemplate { get; }
		public int NewVersion { get; }
		public DateTime OccurredOn { get; }
	}
}

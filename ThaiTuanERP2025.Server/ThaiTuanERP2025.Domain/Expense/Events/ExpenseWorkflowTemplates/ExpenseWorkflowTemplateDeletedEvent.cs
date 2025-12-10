using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpenseWorkflowTemplates
{
	public sealed class ExpenseWorkflowTemplateDeletedEvent : IDomainEvent
	{
		public ExpenseWorkflowTemplateDeletedEvent(ExpenseWorkflowTemplate workflowTemplate)
		{
			WorkflowTemplate = workflowTemplate;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseWorkflowTemplate WorkflowTemplate { get; }
		public DateTime OccurredOn { get; }
	}
}

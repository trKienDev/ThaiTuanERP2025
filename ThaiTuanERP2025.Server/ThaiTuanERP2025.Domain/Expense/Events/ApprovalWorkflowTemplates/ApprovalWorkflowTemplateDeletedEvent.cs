using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalWorkflowTemplates
{
	public sealed class ApprovalWorkflowTemplateDeletedEvent : IDomainEvent
	{
		public ApprovalWorkflowTemplateDeletedEvent(ExpenseWorkflowTemplate workflowTemplate)
		{
			WorkflowTemplate = workflowTemplate;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseWorkflowTemplate WorkflowTemplate { get; }
		public DateTime OccurredOn { get; }
	}
}

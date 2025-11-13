using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpenseWorkflowInstances
{
	public sealed class ExpenseWorkflowInstanceStatusChangedEvent : IDomainEvent
	{
		public ExpenseWorkflowInstanceStatusChangedEvent(ExpenseWorkflowInstance workflowInstance, WorkflowStatus newStatus)
		{
			WorkflowInstance = workflowInstance;
			NewStatus = newStatus;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseWorkflowInstance WorkflowInstance { get; }
		public WorkflowStatus NewStatus { get; }
		public DateTime OccurredOn { get; }
	}
}

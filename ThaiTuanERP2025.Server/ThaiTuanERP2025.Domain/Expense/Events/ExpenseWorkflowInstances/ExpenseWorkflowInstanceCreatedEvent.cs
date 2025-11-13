using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpenseWorkflowInstances
{
	public sealed class ExpenseWorkflowInstanceCreatedEvent : IDomainEvent
	{
		public ExpenseWorkflowInstanceCreatedEvent(ExpenseWorkflowInstance workflowInstance)
		{
			WorkflowInstance = workflowInstance;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseWorkflowInstance WorkflowInstance { get; }
		public DateTime OccurredOn { get; }
	}
}

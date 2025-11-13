using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpenseWorkflowInstances
{
	public sealed class ExpenseWorkflowInstanceStepChangedEvent : IDomainEvent
	{
		public ExpenseWorkflowInstanceStepChangedEvent(ExpenseWorkflowInstance workflowInstance, int newStepOrder)
		{
			WorkflowInstance = workflowInstance;
			NewStepOrder = newStepOrder;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseWorkflowInstance WorkflowInstance { get; }
		public int NewStepOrder { get; }
		public DateTime OccurredOn { get; }
	}
}

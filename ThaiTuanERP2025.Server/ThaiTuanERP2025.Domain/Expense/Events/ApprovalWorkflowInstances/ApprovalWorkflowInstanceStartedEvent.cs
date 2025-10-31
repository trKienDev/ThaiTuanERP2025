using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalWorkflowInstances
{
	public sealed class ApprovalWorkflowInstanceStartedEvent : IDomainEvent
	{
		public ApprovalWorkflowInstanceStartedEvent(ExpenseWorkflowInstance workflowInstance)
		{
			WorkflowInstance = workflowInstance;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseWorkflowInstance WorkflowInstance { get; }
		public DateTime OccurredOn { get; }
	}
}

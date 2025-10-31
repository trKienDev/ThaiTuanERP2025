using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalWorkflowInstances
{
	public sealed class ApprovalWorkflowInstanceStatusChangedEvent : IDomainEvent
	{
		public ApprovalWorkflowInstanceStatusChangedEvent(ExpenseWorkflowInstance workflowInstance, WorkflowStatus newStatus)
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

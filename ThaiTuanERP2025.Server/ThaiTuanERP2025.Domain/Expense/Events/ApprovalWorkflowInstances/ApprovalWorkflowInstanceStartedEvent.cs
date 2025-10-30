using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalWorkflowInstances
{
	public sealed class ApprovalWorkflowInstanceStartedEvent : IDomainEvent
	{
		public ApprovalWorkflowInstanceStartedEvent(ApprovalWorkflowInstance workflowInstance)
		{
			WorkflowInstance = workflowInstance;
			OccurredOn = DateTime.UtcNow;
		}

		public ApprovalWorkflowInstance WorkflowInstance { get; }
		public DateTime OccurredOn { get; }
	}
}

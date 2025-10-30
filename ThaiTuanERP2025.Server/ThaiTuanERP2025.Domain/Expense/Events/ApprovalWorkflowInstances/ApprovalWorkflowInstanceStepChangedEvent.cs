using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalWorkflowInstances
{
	public sealed class ApprovalWorkflowInstanceStepChangedEvent : IDomainEvent
	{
		public ApprovalWorkflowInstanceStepChangedEvent(ApprovalWorkflowInstance workflowInstance, int newStepOrder)
		{
			WorkflowInstance = workflowInstance;
			NewStepOrder = newStepOrder;
			OccurredOn = DateTime.UtcNow;
		}

		public ApprovalWorkflowInstance WorkflowInstance { get; }
		public int NewStepOrder { get; }
		public DateTime OccurredOn { get; }
	}
}

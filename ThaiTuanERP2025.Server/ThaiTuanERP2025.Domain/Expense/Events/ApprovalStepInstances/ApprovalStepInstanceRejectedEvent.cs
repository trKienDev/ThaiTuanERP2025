using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalStepInstances
{
	public sealed class ApprovalStepInstanceRejectedEvent : IDomainEvent
	{
		public ApprovalStepInstanceRejectedEvent(ApprovalStepInstance stepInstance)
		{
			StepInstance = stepInstance;
			OccurredOn = DateTime.UtcNow;
		}

		public ApprovalStepInstance StepInstance { get; }
		public DateTime OccurredOn { get; }
	}
}

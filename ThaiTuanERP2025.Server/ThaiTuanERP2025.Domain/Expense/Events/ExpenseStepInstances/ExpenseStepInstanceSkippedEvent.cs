using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalStepInstances
{
	public sealed class ExpenseStepInstanceSkippedEvent : IDomainEvent
	{
		public ExpenseStepInstanceSkippedEvent(ExpenseStepInstance stepInstance)
		{
			StepInstance = stepInstance;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseStepInstance StepInstance { get; }
		public DateTime OccurredOn { get; }
	}
}

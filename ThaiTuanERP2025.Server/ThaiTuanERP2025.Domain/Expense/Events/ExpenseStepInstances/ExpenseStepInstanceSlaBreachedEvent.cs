using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalStepInstances
{
	public sealed class ExpenseStepInstanceSlaBreachedEvent : IDomainEvent
	{
		public ExpenseStepInstanceSlaBreachedEvent(ExpenseStepInstance stepInstance)
		{
			StepInstance = stepInstance;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseStepInstance StepInstance { get; }
		public DateTime OccurredOn { get; }
	}
}

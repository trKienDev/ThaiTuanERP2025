using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalStepInstances
{

	public sealed class ExpenseStepInstanceApprovedEvent : IDomainEvent
	{
		public ExpenseStepInstanceApprovedEvent(ExpenseStepInstance stepInstance)
		{
			StepInstance = stepInstance;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseStepInstance StepInstance { get; }
		public DateTime OccurredOn { get; }
	}
}

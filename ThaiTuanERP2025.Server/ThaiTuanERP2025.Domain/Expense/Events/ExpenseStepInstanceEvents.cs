using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Expense.Events
{
	public class ExpenseStepInstanceEventBase : IDomainEvent
	{
		public ExpenseStepInstance StepInstance { get; }
		public DateTime OccurredOn { get; }
		protected ExpenseStepInstanceEventBase(ExpenseStepInstance stepInstance)
		{
			StepInstance = stepInstance;
			OccurredOn = DateTime.Now;
		}
	}

	public sealed class ExpenseStepInstanceActivatedEvent : ExpenseStepInstanceEventBase
	{
		public ExpenseStepInstanceActivatedEvent(ExpenseStepInstance stepInstance) : base(stepInstance) { }	
	}

	public sealed class ExpenseStepInstanceApprovedEvent : ExpenseStepInstanceEventBase
	{
		public ExpenseStepInstanceApprovedEvent(ExpenseStepInstance stepInstance) : base(stepInstance) { }
	}
}

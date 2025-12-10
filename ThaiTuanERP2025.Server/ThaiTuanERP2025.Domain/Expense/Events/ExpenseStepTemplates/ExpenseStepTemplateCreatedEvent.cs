using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpenseStepTemplates
{
	public sealed class ExpenseStepTemplateCreatedEvent : IDomainEvent
	{
		public ExpenseStepTemplateCreatedEvent(ExpenseStepTemplate stepTemplate)
		{
			StepTemplate = stepTemplate;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseStepTemplate StepTemplate { get; }
		public DateTime OccurredOn { get; }
	}
}

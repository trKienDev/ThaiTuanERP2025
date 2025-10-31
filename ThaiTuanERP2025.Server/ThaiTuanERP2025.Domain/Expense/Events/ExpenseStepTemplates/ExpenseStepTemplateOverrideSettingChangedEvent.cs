using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpenseStepTemplates
{
	public sealed class ExpenseStepTemplateOverrideSettingChangedEvent : IDomainEvent
	{
		public ExpenseStepTemplateOverrideSettingChangedEvent(ExpenseStepTemplate stepTemplate)
		{
			StepTemplate = stepTemplate;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpenseStepTemplate StepTemplate { get; }
		public DateTime OccurredOn { get; }
	}
}

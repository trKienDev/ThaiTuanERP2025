using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalStepTemplates
{
	public sealed class ApprovalStepTemplateOverrideSettingChangedEvent : IDomainEvent
	{
		public ApprovalStepTemplateOverrideSettingChangedEvent(ApprovalStepTemplate stepTemplate)
		{
			StepTemplate = stepTemplate;
			OccurredOn = DateTime.UtcNow;
		}

		public ApprovalStepTemplate StepTemplate { get; }
		public DateTime OccurredOn { get; }
	}
}

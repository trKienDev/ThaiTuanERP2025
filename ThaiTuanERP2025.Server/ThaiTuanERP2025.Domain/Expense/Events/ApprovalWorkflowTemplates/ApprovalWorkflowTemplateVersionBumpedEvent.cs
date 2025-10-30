using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ApprovalWorkflowTemplates
{
	public sealed class ApprovalWorkflowTemplateVersionBumpedEvent : IDomainEvent
	{
		public ApprovalWorkflowTemplateVersionBumpedEvent(ApprovalWorkflowTemplate workflowTemplate, int newVersion)
		{
			WorkflowTemplate = workflowTemplate;
			NewVersion = newVersion;
			OccurredOn = DateTime.UtcNow;
		}

		public ApprovalWorkflowTemplate WorkflowTemplate { get; }
		public int NewVersion { get; }
		public DateTime OccurredOn { get; }
	}
}

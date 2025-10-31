using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Expense.Events.ApprovalStepTemplates;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpenseStepTemplate : AuditableEntity
	{
		public Guid WorkflowTemplateId { get; private set; }
		public ExpenseWorkflowTemplate WorkflowTemplate { get; private set; } = null!;
		public string Name { get; private set; } = string.Empty;
		public int Order { get; private set; }
		public ExpenseFlowType FlowType { get; private set; }
		public int SlaHours { get; private set; }
		public ApproverMode ApproverMode { get; private set; }
		public string? FixedApproverIdsJson { get; private set; }
		public string? ResolverKey { get; private set; }
		public string? ResolverParamsJson { get; private set; }
		public bool AllowOverride { get; private set; }

		#region Constructors
		private ExpenseStepTemplate() { }
		public ExpenseStepTemplate(
			Guid workflowTemplateId,
			string name,
			int order,
			ExpenseFlowType flowType,
			int slaHours,
			ApproverMode approverMode,
			string? fixedApproverIdsJson = null,
			string? resolverKey = null,
			string? resolverParamsJson = null,
			bool allowOverride = false
		)
		{
			Guard.AgainstDefault(workflowTemplateId, nameof(workflowTemplateId));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstInvalidEnumValue(flowType, nameof(flowType));
			Guard.AgainstInvalidEnumValue(approverMode, nameof(approverMode));
			Guard.AgainstNegative(slaHours, nameof(slaHours));

			Id = Guid.NewGuid();
			WorkflowTemplateId = workflowTemplateId;
			Name = name.Trim();
			Order = order;
			FlowType = flowType;
			SlaHours = slaHours;
			ApproverMode = approverMode;
			FixedApproverIdsJson = fixedApproverIdsJson;
			ResolverKey = resolverKey;
			ResolverParamsJson = resolverParamsJson;
			AllowOverride = allowOverride;

			AddDomainEvent(new ApprovalStepTemplateCreatedEvent(this));
		}
		#endregion

		#region Domain Behaviors
		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new ApprovalStepTemplateRenamedEvent(this));
		}

		public void ChangeApproverMode(ApproverMode newMode, string? fixedApproversJson = null, string? resolverKey = null)
		{
			Guard.AgainstInvalidEnumValue(newMode, nameof(newMode));
			ApproverMode = newMode;
			FixedApproverIdsJson = fixedApproversJson;
			ResolverKey = resolverKey;
			AddDomainEvent(new ApprovalStepTemplateApproverModeChangedEvent(this));
		}

		public void UpdateSla(int newSlaHours)
		{
			Guard.AgainstNegative(newSlaHours, nameof(newSlaHours));
			SlaHours = newSlaHours;
			AddDomainEvent(new ApprovalStepTemplateSlaUpdatedEvent(this));
		}

		public void ToggleOverride(bool allow)
		{
			AllowOverride = allow;
			AddDomainEvent(new ApprovalStepTemplateOverrideSettingChangedEvent(this));
		}
		#endregion
	}
}

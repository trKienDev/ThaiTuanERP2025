using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Expense.Events.ExpenseStepTemplates;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpenseStepTemplate : BaseEntity
	{
		public Guid WorkflowTemplateId { get; private set; }
		public ExpenseWorkflowTemplate WorkflowTemplate { get; private set; } = null!;
		public string Name { get; private set; } = string.Empty;
		public int Order { get; private set; }
		public ExpenseFlowType FlowType { get; private set; }
		public int SlaHours { get; private set; }
		public ExpenseApproveMode ExpenseApproveMode { get; private set; }
		public string? FixedApproverIdsJson { get; private set; }

		// Chứa tên rule để xác định người duyệt động (creator-manager, "amount-based")
		public string? ResolverKey { get; private set; }
		public string? ResolverParamsJson { get; private set; }

		#region Constructors
		private ExpenseStepTemplate() { }
		public ExpenseStepTemplate(
			Guid workflowTemplateId,
			string name,
			int order,
			ExpenseFlowType flowType,
			int slaHours,
			ExpenseApproveMode expenseApproveMode,
			string? fixedApproverIdsJson = null,
			string? resolverKey = null,
			string? resolverParamsJson = null
		)
		{
			Guard.AgainstDefault(workflowTemplateId, nameof(workflowTemplateId));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstInvalidEnumValue(flowType, nameof(flowType));
			Guard.AgainstInvalidEnumValue(expenseApproveMode, nameof(expenseApproveMode));
			Guard.AgainstNegative(slaHours, nameof(slaHours));

			Id = Guid.NewGuid();
			WorkflowTemplateId = workflowTemplateId;
			Name = name.Trim();
			Order = order;
			FlowType = flowType;
			SlaHours = slaHours;
			ExpenseApproveMode = expenseApproveMode;
			FixedApproverIdsJson = fixedApproverIdsJson;
			ResolverKey = resolverKey;
			ResolverParamsJson = resolverParamsJson;

			AddDomainEvent(new ExpenseStepTemplateCreatedEvent(this));
		}
		#endregion

		#region Domain Behaviors
		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new ExpenseStepTemplateRenamedEvent(this));
		}

		public void ChangeApproverMode(ExpenseApproveMode newMode, string? fixedApproversJson = null, string? resolverKey = null)
		{
			Guard.AgainstInvalidEnumValue(newMode, nameof(newMode));
			ExpenseApproveMode = newMode;
			FixedApproverIdsJson = fixedApproversJson;
			ResolverKey = resolverKey;
			AddDomainEvent(new ExpenseStepTemplateApproverModeChangedEvent(this));
		}

		public void UpdateSla(int newSlaHours)
		{
			Guard.AgainstNegative(newSlaHours, nameof(newSlaHours));
			SlaHours = newSlaHours;
			AddDomainEvent(new ExpenseStepTemplateSlaUpdatedEvent(this));
		}
		#endregion
	}
}

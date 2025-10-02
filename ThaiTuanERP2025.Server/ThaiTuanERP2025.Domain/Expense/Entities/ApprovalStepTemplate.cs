using System.ComponentModel.DataAnnotations;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalStepTemplate : AuditableEntity
	{
		// FK → Workflow Template
		public Guid WorkflowTemplateId { get; private set; }
		public ApprovalWorkflowTemplate WorkflowTemplate { get; private set; } = null!;

		[MaxLength(200)]
		public string Name { get; private set; } = string.Empty;

		/// <summary>Thứ tự hiển thị/thực thi (unique trong một template)</summary>
		public int Order { get; private set; }

		public FlowType FlowType { get; private set; } // single / one-of-n
		public int SlaHours { get; private set; }

		public ApproverMode ApproverMode { get; private set; } // standard / condition

		/// <summary>
		/// JSON array of GUIDs (["...","..."]) khi ApproverMode=Standard.
		/// Dùng string để map NVARCHAR(MAX). Validate JSON qua CHECK constraint.
		/// </summary>
		public string? FixedApproverIdsJson { get; private set; }

		/// <summary>
		/// Khóa resolver (vd: 'creator-manager', 'creator-department-manager', …) khi ApproverMode=Condition.
		/// </summary>
		[MaxLength(100)]
		public string? ResolverKey { get; private set; }

		/// <summary>
		/// Tham số hóa resolver (JSON) — tùy chọn.
		/// </summary>
		public string? ResolverParamsJson { get; private set; }

		/// <summary>
		/// Cho phép người lập override người duyệt (khi condition) trong phạm vi candidates hợp lệ.
		/// </summary>
		public bool AllowOverride { get; private set; }

		private ApprovalStepTemplate() { }

		public ApprovalStepTemplate(
			Guid workflowTemplateId, 
			string name, 
			int order,
			FlowType flowType,
			int slaHours,
			ApproverMode approverMode,
			string? fixedApproverIdsJson = null,
			string? resolverKey = null,
			string? resolverParamsJson = null,
			bool allowOverride = false
		) {
			WorkflowTemplateId = workflowTemplateId;
			Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name cannot be null or empty.", nameof(name)) : name;
			Order = order;
			FlowType = flowType;
			SlaHours = slaHours >= 0 ? slaHours : throw new ArgumentOutOfRangeException(nameof(slaHours), "SlaHours cannot be negative.");
			ApproverMode = approverMode;
			FixedApproverIdsJson = fixedApproverIdsJson;
			ResolverKey = resolverKey;
			ResolverParamsJson = resolverParamsJson;
			AllowOverride = allowOverride;
		}

		public void MarkDeleted() => IsDeleted = true;
	}
}

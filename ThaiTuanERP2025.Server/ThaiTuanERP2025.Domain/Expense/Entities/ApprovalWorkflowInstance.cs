using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalWorkflowInstance : AuditableEntity
	{

		public Guid TemplateId { get; private set; }
		public int TemplateVersion { get; private set; }
		public string DocumentType { get; private set; } = string.Empty;
		public Guid DocumentId { get; private set; }

		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		public WorkflowStatus Status { get; private set; } = WorkflowStatus.Draft;
		public int? CurrentStepOrder { get; private set; }

		public string? RawJson { get; private set; } // snapshot nhanh

		// Denormalized dimensions for reporting
		public decimal? Amount { get; private set; }
		public string? Currency { get; private set; }
		public string? BudgetCode { get; private set; }
		public string? CostCenter { get; private set; }

		public DateTime? ApprovedAt { get; private set; }
		public Guid? ApprovedBy { get; private set; }

		// Navigation
		public ICollection<ApprovalStepInstance> Steps { get; private set; } = new List<ApprovalStepInstance>();

		private ApprovalWorkflowInstance() { }

		public ApprovalWorkflowInstance(
		    Guid templateId, int templateVersion, string documentType, Guid documentId,
		    Guid createdByUserId, decimal? amount = null, string? currency = null, string? budgetCode = null, string? costCenter = null,
		    string? rawJson = null)
		{
			Id = Guid.NewGuid();
			TemplateId = templateId;
			TemplateVersion = templateVersion;
			DocumentType = documentType;
			DocumentId = documentId;
			CreatedByUserId = createdByUserId;
			CreatedAt = DateTime.UtcNow;
			Status = WorkflowStatus.Draft;
			Amount = amount;
			Currency = currency;
			BudgetCode = budgetCode;
			CostCenter = costCenter;
			RawJson = rawJson;
		}

		public void Start(int firstOrder)
		{
			Status = WorkflowStatus.InProgress;
			CurrentStepOrder = firstOrder;
		}

		public void SetStatus(WorkflowStatus status) => Status = status;
		public void SetCurrentStep(int? order) => CurrentStepOrder = order;

		public void MarkApproved(string? reason = null) {
			if (Status is WorkflowStatus.Approved or WorkflowStatus.Expired)
				return;
			Status = WorkflowStatus.Approved;
			ApprovedAt = DateTime.UtcNow;
			AppendHistory("completed", reason);
		}

		public void MarkInProgress() {
			Status = WorkflowStatus.InProgress;
		}

		private void AppendHistory(string action, string? note)
		{
			// tuỳ bạn đang lưu HistoryJson thế nào, ở đây chỉ gợi ý
			// HistoryJsonAdd(new { at = DateTime.UtcNow, action, note });
		}
	}
}

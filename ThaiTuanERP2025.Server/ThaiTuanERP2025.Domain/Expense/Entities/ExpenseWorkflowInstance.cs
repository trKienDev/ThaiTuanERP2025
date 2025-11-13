using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Expense.Events.ApprovalWorkflowInstances;
using ThaiTuanERP2025.Domain.Expense.Events.ExpenseWorkflowInstances;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpenseWorkflowInstance : BaseEntity
	{
		#region Constructors
		private ExpenseWorkflowInstance() { }
		public ExpenseWorkflowInstance (
			Guid templateId,
			int templateVersion,
			string documentType,
			Guid documentId,
			Guid createdByUserId,
			decimal? amount = null,
			string? currency = null,
			string? budgetCode = null,
			string? costCenter = null,
			string? rawJson = null
		) {
			Guard.AgainstDefault(templateId, nameof(templateId));
			Guard.AgainstDefault(documentId, nameof(documentId));
			Guard.AgainstNullOrWhiteSpace(documentType, nameof(documentType));
			Guard.AgainstNegative(templateVersion, nameof(templateVersion));

			Id = Guid.NewGuid();
			TemplateId = templateId;
			TemplateVersion = templateVersion;
			DocumentType = documentType;
			DocumentId = documentId;
			Status = WorkflowStatus.Draft;
			Amount = amount;
			Currency = currency;
			BudgetCode = budgetCode;
			CostCenter = costCenter;
			RawJson = rawJson;

			AddDomainEvent(new ExpenseWorkflowInstanceCreatedEvent(this));
		}
		#endregion

		#region Properties
		public Guid TemplateId { get; private set; }
		public int TemplateVersion { get; private set; }
		public string DocumentType { get; private set; } = string.Empty;
		public Guid DocumentId { get; private set; }
		public WorkflowStatus Status { get; private set; } = WorkflowStatus.Draft;
		public int CurrentStepOrder { get; private set; } = 1;
		public string? RawJson { get; private set; }

		public decimal? Amount { get; private set; }
		public string? Currency { get; private set; }
		public string? BudgetCode { get; private set; }
		public string? CostCenter { get; private set; }

		public DateTime? ApprovedAt { get; private set; }
		public Guid? ApprovedBy { get; private set; }

		public ICollection<ExpenseStepInstance> Steps { get; private set; } = new List<ExpenseStepInstance>();
		#endregion

		#region Domain Behaviors

		public void Start(int firstOrder)
		{
			if (Status != WorkflowStatus.Draft)
				throw new DomainException("Workflow chỉ có thể khởi động khi ở trạng thái Draft.");

			if (!Steps.Any())
				throw new DomainException("Workflow phải có ít nhất một bước phê duyệt để bắt đầu.");

			Status = WorkflowStatus.InProgress;
			CurrentStepOrder = firstOrder;

			AddDomainEvent(new ExpenseWorkflowInstanceStartedEvent(this));
		}

		public void MoveToNextStep(int nextOrder)
		{
			if (Status != WorkflowStatus.InProgress)
				throw new DomainException("Không thể chuyển bước khi workflow chưa ở trạng thái InProgress.");

			CurrentStepOrder = nextOrder;
			AddDomainEvent(new ExpenseWorkflowInstanceStepChangedEvent(this, nextOrder));
		}

		public void SetCurrentStepOrder(int newOrder)
		{
			if (Status != WorkflowStatus.InProgress)
				throw new DomainException("Chỉ có thể thay đổi bước hiện tại khi workflow đang ở trạng thái InProgress.");

			if (newOrder <= 0)
				throw new DomainException("Thứ tự bước phải lớn hơn 0.");

			if (newOrder == CurrentStepOrder)
				return;

			if (newOrder < CurrentStepOrder)
				throw new DomainException("Không thể quay lại bước trước trong workflow.");

			// Cập nhật
			CurrentStepOrder = newOrder;

			AddDomainEvent(new ExpenseWorkflowInstanceStepChangedEvent(this, newOrder));
		}

		public void MarkApproved(Guid byUserId, string? reason = null)
		{
			if (Status == WorkflowStatus.Approved)
				return;

			if (Status != WorkflowStatus.InProgress)
				throw new DomainException("Chỉ có thể phê duyệt workflow đang xử lý.");

			Status = WorkflowStatus.Approved;
			ApprovedBy = byUserId;
			ApprovedAt = DateTime.UtcNow;

			AddDomainEvent(new ExpenseWorkflowInstanceApprovedEvent(this));
		}

		public void MarkInProgress()
		{
			Status = WorkflowStatus.InProgress;
			AddDomainEvent(new ExpenseWorkflowInstanceStatusChangedEvent(this, WorkflowStatus.InProgress));
		}

		#endregion
	}
}

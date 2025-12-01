using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpenseWorkflowInstance : BaseEntity
	{
		#region Constructors
		private ExpenseWorkflowInstance() { }
		public ExpenseWorkflowInstance (
			Guid templateId,
			int templateVersion,
			DocumentType documentType,
			Guid documentId
		) {
			Guard.AgainstDefault(templateId, nameof(templateId));
			Guard.AgainstDefault(documentId, nameof(documentId));
			Guard.AgainstNegative(templateVersion, nameof(templateVersion));

			Id = Guid.NewGuid();
			TemplateId = templateId;
			TemplateVersion = templateVersion;
			DocumentType = documentType;
			DocumentId = documentId;
			Status = ExpenseWorkflowStatus.Draft;
		}
		#endregion

		#region Properties
		public Guid TemplateId { get; private set; }
		public int TemplateVersion { get; private set; }
		public DocumentType DocumentType { get; private set; }
		public Guid DocumentId { get; private set; }
		public ExpenseWorkflowStatus Status { get; private set; } = ExpenseWorkflowStatus.Draft;
		public int CurrentStepOrder { get; private set; } = 1;

		public ICollection<ExpenseStepInstance> Steps { get; private set; } = new List<ExpenseStepInstance>();
		#endregion

		#region Domain Behaviors

		internal void Start()
		{
			if (Status != ExpenseWorkflowStatus.Draft)
				throw new DomainException("Workflow chỉ có thể khởi động khi ở trạng thái Draft.");

			if (!Steps.Any())
				throw new DomainException("Workflow phải có ít nhất một bước phê duyệt để bắt đầu.");

			Status = ExpenseWorkflowStatus.InProgress;

			ActivateFirstStep();
		}

		internal ExpenseStepInstance GetFirstStep()
		{
			return Steps.OrderBy(s => s.Order).FirstOrDefault()
				?? throw new DomainException("Workflow không có bước nào.");
		}

		internal ExpenseStepInstance GetCurrentStep()
		{
                        var step = Steps.FirstOrDefault(s => s.Order == CurrentStepOrder);

                        return step ?? throw new DomainException($"Không tìm thấy bước duyệt (Order={CurrentStepOrder}) trong workflow.");
                }

                internal ExpenseStepInstance? GetStepByOrder(int order)
                {
                        return Steps.FirstOrDefault(s => s.Order == order);
                }

                internal void ActivateFirstStep()
		{
			var first = GetFirstStep();
			first.Activate();
		}

		internal void MoveToNextStep(int nextOrder)
		{
			if (Status != ExpenseWorkflowStatus.InProgress)
				throw new DomainException("Không thể chuyển bước khi workflow chưa ở trạng thái InProgress.");

			CurrentStepOrder = nextOrder;
		}

                internal void ActivateNextStep()
                {
                        var nextOrder = CurrentStepOrder + 1;
                        var nextStep = GetStepByOrder(nextOrder);

                        if (nextStep == null)
                        {
				// Không có bước nữa → workflow hoàn tất
				MarkApproved(Guid.Empty, "Auto-approved — no more steps.");
				return;
                        }

			SetCurrentStepOrder(nextOrder);
                        nextStep.Activate();
                }

                internal void SetCurrentStepOrder(int newOrder)
		{
			if (Status != ExpenseWorkflowStatus.InProgress)
				throw new DomainException("Chỉ có thể thay đổi bước hiện tại khi workflow đang ở trạng thái InProgress.");

			if (newOrder <= 0)
				throw new DomainException("Thứ tự bước phải lớn hơn 0.");

			if (newOrder == CurrentStepOrder)
				return;

			if (newOrder < CurrentStepOrder)
				throw new DomainException("Không thể quay lại bước trước trong workflow.");

			// Cập nhật
			CurrentStepOrder = newOrder;
		}

		internal void MarkApproved(Guid byUserId, string? reason = null)
		{
			if (Status == ExpenseWorkflowStatus.Approved)
				return;

			if (Status != ExpenseWorkflowStatus.InProgress)
				throw new DomainException("Chỉ có thể phê duyệt workflow đang xử lý.");

			Status = ExpenseWorkflowStatus.Approved;
		}

		internal void MarkInProgress()
		{
			Status = ExpenseWorkflowStatus.InProgress;
		}

		#endregion
	}
}

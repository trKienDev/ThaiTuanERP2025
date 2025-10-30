using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	using ThaiTuanERP2025.Domain.Approvals.Events;
	using ThaiTuanERP2025.Domain.Common;
	using ThaiTuanERP2025.Domain.Exceptions;
	using ThaiTuanERP2025.Domain.Expense.Events.ApprovalStepInstances;

	public class ApprovalStepInstance : AuditableEntity
	{
		private ApprovalStepInstance() { }

		public ApprovalStepInstance(
			Guid workflowInstanceId, Guid? templateStepId, string name, int order,
			FlowType flowType, int slaHours, ApproverMode approverMode,
			string? candidatesJson, Guid? defaultApproverId, Guid? selectedApproverId,
			StepStatus status = StepStatus.Pending)
		{
			Guard.AgainstDefault(workflowInstanceId, nameof(workflowInstanceId));
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstInvalidEnumValue(flowType, nameof(flowType));
			Guard.AgainstInvalidEnumValue(approverMode, nameof(approverMode));

			Id = Guid.NewGuid();
			WorkflowInstanceId = workflowInstanceId;
			TemplateStepId = templateStepId;
			Name = name.Trim();
			Order = order;
			FlowType = flowType;
			SlaHours = slaHours;
			ApproverMode = approverMode;
			ResolvedApproverCandidatesJson = candidatesJson;
			DefaultApproverId = defaultApproverId;
			SelectedApproverId = selectedApproverId;
			Status = status;

			AddDomainEvent(new ApprovalStepCreatedEvent(this));
		}

		public Guid WorkflowInstanceId { get; private set; }
		public ApprovalWorkflowInstance WorkflowInstance { get; private set; } = null!;
		public Guid? TemplateStepId { get; private set; }

		public string Name { get; private set; } = string.Empty;
		public int Order { get; private set; }
		public FlowType FlowType { get; private set; }
		public int SlaHours { get; private set; }
		public ApproverMode ApproverMode { get; private set; }

		public string? ResolvedApproverCandidatesJson { get; private set; }
		public Guid? DefaultApproverId { get; private set; }
		public Guid? SelectedApproverId { get; private set; }

		public StepStatus Status { get; private set; } = StepStatus.Pending;

		public DateTime? StartedAt { get; private set; }
		public DateTime? DueAt { get; private set; }

		public DateTime? ApprovedAt { get; private set; }
		public Guid? ApprovedBy { get; private set; }
		public User? ApprovedByUser { get; private set; }

		public DateTime? RejectedAt { get; private set; }
		public Guid? RejectedBy { get; private set; }
		public User? RejectedByUser { get; private set; }

		public string? Comments { get; private set; }
		public bool SlaBreached { get; private set; }

		public string? HistoryJson { get; private set; }

		#region Domain Behaviors
		public void Activate(DateTime utcNow)
		{
			if (Status != StepStatus.Pending)
				throw new DomainException("Chỉ bước đang chờ mới được kích hoạt.");
			Status = StepStatus.Waiting;
			StartedAt = utcNow;
			DueAt = utcNow.AddHours(SlaHours);
			AddDomainEvent(new ApprovalStepInstanceActivatedEvent(this));
		}

		public void Approve(Guid by, DateTime utcNow)
		{
			if (Status != StepStatus.Waiting)
				throw new DomainException("Không thể duyệt bước không ở trạng thái 'Waiting'.");
			Status = StepStatus.Approved;
			ApprovedBy = by;
			ApprovedAt = utcNow;
			AddDomainEvent(new ApprovalStepInstanceApprovedEvent(this));
		}

		public void Reject(Guid by, string? comment, DateTime utcNow)
		{
			if (Status != StepStatus.Waiting)
				throw new DomainException("Không thể từ chối bước không ở trạng thái 'Waiting'.");
			Status = StepStatus.Rejected;
			RejectedBy = by;
			RejectedAt = utcNow;
			Comments = comment;
			AddDomainEvent(new ApprovalStepInstanceRejectedEvent(this));
		}

		public void Skip(string? reason)
		{
			if (Status is StepStatus.Approved or StepStatus.Skipped)
				return;
			Status = StepStatus.Skipped;
			Comments = reason;
			AddDomainEvent(new ApprovalStepInstanceSkippedEvent(this));
		}

		public void MarkSlaBreached()
		{
			if (SlaBreached) return;
			SlaBreached = true;
			AddDomainEvent(new ApprovalStepInstanceSlaBreachedEvent(this));
		}

		public void SetResolvedApproverCandidates(IEnumerable<Guid> ids)
		{
			ResolvedApproverCandidatesJson =
				System.Text.Json.JsonSerializer.Serialize(ids?.Distinct() ?? Enumerable.Empty<Guid>());
		}
		#endregion
	}

}

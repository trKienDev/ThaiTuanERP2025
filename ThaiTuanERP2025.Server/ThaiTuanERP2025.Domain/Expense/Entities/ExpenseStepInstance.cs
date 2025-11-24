using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Expense.Events.ApprovalStepInstances;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpenseStepInstance : AuditableEntity
	{
		public Guid WorkflowInstanceId { get; private set; }
		public ExpenseWorkflowInstance WorkflowInstance { get; private set; } = null!;
		public Guid? TemplateStepId { get; private set; }

		public string Name { get; private set; } = string.Empty;
		public int Order { get; private set; }
		public ExpenseFlowType FlowType { get; private set; }
		public int SlaHours { get; private set; }
		public ExpenseApproveMode ApproverMode { get; private set; }

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

		#region Constructors
		private ExpenseStepInstance() { }
		public ExpenseStepInstance( 
			Guid workflowInstanceId, Guid? templateStepId, string name, int order,
			ExpenseFlowType flowType, int slaHours, ExpenseApproveMode approverMode,
			string? candidatesJson, Guid? defaultApproverId, Guid? selectedApproverId,
			StepStatus status = StepStatus.Pending
		) {
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

			AddDomainEvent(new ExpenseStepInstanceCreatedEvent(this));
		}
		#endregion

		#region Domain Behaviors
		public void Activate(DateTime utcNow)
		{
			if (Status != StepStatus.Pending)
				throw new DomainException("Chỉ bước đang chờ mới được kích hoạt.");
			Status = StepStatus.Waiting;
			StartedAt = utcNow;
			DueAt = utcNow.AddHours(SlaHours);
			AddDomainEvent(new ExpenseStepInstanceActivatedEvent(this));
		}

		public void Approve(Guid by, DateTime utcNow)
		{
			if (Status != StepStatus.Waiting)
				throw new DomainException("Không thể duyệt bước không ở trạng thái 'Waiting'.");
			Status = StepStatus.Approved;
			ApprovedBy = by;
			ApprovedAt = utcNow;
			AddDomainEvent(new ExpenseStepInstanceApprovedEvent(this));
		}

		public void Reject(Guid by, string? comment, DateTime utcNow)
		{
			if (Status != StepStatus.Waiting)
				throw new DomainException("Không thể từ chối bước không ở trạng thái 'Waiting'.");
			Status = StepStatus.Rejected;
			RejectedBy = by;
			RejectedAt = utcNow;
			Comments = comment;
			AddDomainEvent(new ExpenseStepInstanceRejectedEvent(this));
		}

		public void Skip(string? reason)
		{
			if (Status is StepStatus.Approved or StepStatus.Skipped)
				return;
			Status = StepStatus.Skipped;
			Comments = reason;
			AddDomainEvent(new ExpenseStepInstanceSkippedEvent(this));
		}

		public void MarkSlaBreached()
		{
			if (SlaBreached) return;
			SlaBreached = true;
			AddDomainEvent(new ExpenseStepInstanceSlaBreachedEvent(this));
		}

		public void SetResolvedApproverCandidates(IEnumerable<Guid> ids)
		{
			ResolvedApproverCandidatesJson =
				System.Text.Json.JsonSerializer.Serialize(ids?.Distinct() ?? Enumerable.Empty<Guid>());
		}
		#endregion
	}
}

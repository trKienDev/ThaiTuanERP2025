using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalStepInstance : AuditableEntity
	{
		public Guid WorkflowInstanceId { get; private set; }
		public ApprovalWorkflowInstance WorkflowInstance { get; private set; } = null!;

		public Guid? TemplateStepId { get; private set; }

		public string Name { get; private set; } = string.Empty;
		public int Order { get; private set; }

		public FlowType FlowType { get; private set; }
		public int SlaHours { get; private set; }
		public ApproverMode ApproverMode { get; private set; }

		// JSON
		public string? ResolvedApproverCandidatesJson { get; private set; } // ["guid","guid"]
		public Guid? DefaultApproverId { get; private set; }
		public Guid? SelectedApproverId { get; private set; }

		public StepStatus Status { get; private set; } = StepStatus.Pending;

		public DateTime? StartedAt { get; private set; }
		public DateTime? DueAt { get; private set; }

		public DateTime? ApprovedAt { get; private set; }
		public Guid? ApprovedBy { get; private set; }
		public DateTime? RejectedAt { get; private set; }
		public Guid? RejectedBy { get; private set; }

		public string? Comments { get; private set; }
		public bool SlaBreached { get; private set; } = false;

		public string? HistoryJson { get; private set; } // escalation, notify, reassign…

		private ApprovalStepInstance() { }

		public ApprovalStepInstance(
		    Guid workflowInstanceId, Guid? templateStepId, string name, int order,
		    FlowType flowType, int slaHours, ApproverMode approverMode,
		    string? candidatesJson, Guid? defaultApproverId, Guid? selectedApproverId,
		    StepStatus status = StepStatus.Pending)
		{
			Id = Guid.NewGuid();
			WorkflowInstanceId = workflowInstanceId;
			TemplateStepId = templateStepId;
			Name = name;
			Order = order;
			FlowType = flowType;
			SlaHours = slaHours;
			ApproverMode = approverMode;
			ResolvedApproverCandidatesJson = candidatesJson;
			DefaultApproverId = defaultApproverId;
			SelectedApproverId = selectedApproverId;
			Status = status;
		}

		public void Activate(DateTime utcNow)
		{
			Status = StepStatus.Waiting;
			StartedAt = utcNow;
			DueAt = utcNow.AddHours(SlaHours);
		}

		public void MarkSlaBreached() => SlaBreached = true;

		public void Approve(Guid by, DateTime utcNow)
		{
			Status = StepStatus.Approved;
			ApprovedBy = by;
			ApprovedAt = utcNow;
		}

		public void Reject(Guid by, string? comment, DateTime utcNow)
		{
			Status = StepStatus.Rejected;
			RejectedBy = by;
			RejectedAt = utcNow;
			Comments = comment;
		}

		public void Skip(string? reason)
		{
			Status = StepStatus.Skipped;
			Comments = reason;
		}

		public void SetHistory(string? historyJson) => HistoryJson = historyJson;
	}
}

using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalStepInstance : AuditableEntity
	{
		public Guid FlowInstanceId { get; set; }
		public ApprovalFlowInstance FlowInstance { get; set; } = default!;
		
		public Guid StepDefinitionId { get; set; }	
		public ApprovalStepDefinition StepDefinition { get; set; } = default!;

		public string Name { get; set; } = default!;	
		public int OrderIndex { get; set; }
		public ApprovalStepStatus Status { get; set; } = ApprovalStepStatus.Pending;

		public string CandidatesJson { get; set; } = "[]";
		public Guid? ApprovedByUserId {  get; set; }
		public int ApprovedCount { get; set; } = 0;
		public int RequiredCount { get; set; } = 1;

		public DateTime? StartedAt { get; set; }
		public DateTime? DeadlineAt {  get; set; }
		public DateTime? FinishedAt  {  get; set; }

		public byte[] RowVersion { get; set; } = default!;

	}
}

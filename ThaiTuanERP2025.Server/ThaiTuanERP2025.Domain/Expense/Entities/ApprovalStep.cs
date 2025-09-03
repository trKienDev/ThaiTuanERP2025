using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalStep : AuditableEntity
	{
		public Guid ApprovalWorkflowId { get; set; }
		public ApprovalWorkflow ApprovalWorkflow { get; set; } = default!;

		public string Title { get; set; } = default!;	
		public int Order { get; set; }	
		public ApprovalStepFlowType FlowType { get; set; }
		public int SlaHours { get; set; } = 8;

		public string CandidateJson { get; set; } = "[]";
		public string? Description { get; set; }
	}
}

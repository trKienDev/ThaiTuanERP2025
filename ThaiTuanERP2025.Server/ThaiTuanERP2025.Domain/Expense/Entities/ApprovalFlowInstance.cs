using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalFlowInstance : AuditableEntity
	{
		public Guid FlowDefinitionId { get; set; }
		public ApprovalFlowDefinition FlowDefinition { get; set; } = default!;
		public int DefinitionVersion { get; set; }

		public string DocumentType { get; set; } = default!;
		public Guid DocumentId {  get; set; }

		public ApprovalFlowStatus Status { get; set; } = ApprovalFlowStatus.Pending;
		public int? CurrentStepIndex { get; set; }	
		public DateTime? StartedAt { get; set; }	
		public DateTime? FinishedAt { get; set; }

		public ICollection<ApprovalStepInstance> Steps {  get; set; } = new List<ApprovalStepInstance>();
		public byte[] RowVersion { get; set; } = default!;
	}
}

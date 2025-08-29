using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalFlowDefinition : AuditableEntity
	{
		public string Name { get; set; } = default!;
		public string Description { get; set; } = string.Empty;
		public string DocumentType { get; set; } = default!;
		public int Version { get; set; }
		public bool IsActive { get; set; }
		public DateTime? EffectiveFrom { get; set; }
		public DateTime? EffectiveTo { get; set; }

		public ICollection<ApprovalStepDefinition> Steps {  get; set; } = new List<ApprovalStepDefinition>();
		public byte[] RowVersion { get; set; } = default!;

	}
}

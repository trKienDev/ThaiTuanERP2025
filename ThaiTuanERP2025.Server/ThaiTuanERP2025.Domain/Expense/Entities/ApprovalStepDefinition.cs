using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalStepDefinition : AuditableEntity
	{
		public Guid FlowDefinitionId { get; set; }
		public ApprovalFlowDefinition FlowDefinition { get; set; } = default!;
		
		public string Name { get; set; } = default!;
		public int OrderIndex { get; set; }
		public ApprovalResolveType ResolverType { get; set; }
		public string ResolverParamsJson { get; set; } = "{}";
		public ApprovalStepMode Mode { get; set; } = ApprovalStepMode.Parrallel;
		public int RequiredCount { get; set; } = 1;
		public int? SlaHours { get; set; }

		public byte[] RowVersion { get; set; } = default!;
	}
}

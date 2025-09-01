using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class MyPendingApprovalDto
	{
		public Guid StepInstanceId { get; set; }
		public Guid FlowInstanceId { get; set; }
		public string DocumentType { get; set; } = default!;
		public Guid DocumentId { get; set; }

		public string StepName { get; set; } = default!;
		public int OrderIndex	{ get; set; }
		public DateTime? StartedAt {  get; set; }	
		public DateTime? DeadlineAt { get; set; }
	}
}

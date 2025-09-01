using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class FlowInstanceDto
	{
		public Guid Id { get; set; }
		public string DocumentType { get; set; } = default!;
		public Guid DocumentId {  get; set; }
		public string Status { get; set; } = default!;
		public DateTime? StartedAt {  get; set; }
		public DateTime? FinishedAt { get; set; }
		public IReadOnlyList<FlowStepDto> Steps { get; set; } = Array.Empty<FlowStepDto>();
	}
}

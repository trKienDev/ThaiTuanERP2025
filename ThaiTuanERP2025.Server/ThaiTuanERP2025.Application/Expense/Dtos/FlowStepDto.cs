using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class FlowStepDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = default!;
		public int OrderIndex {  get; set; }
		public string Status { get; set; } = default!;
		public Guid? ApprovedByUserId { get; set; }
		public DateTime? StartedAt { get; set; }
		public DateTime? DeadlineAt {  get; set; }
		public DateTime? FinishedAt { get; set; }

		// show candidates
		public IReadOnlyList<Guid> Candidates {  get; set; } = Array.Empty<Guid>();	

		public IReadOnlyList<StepActionDto> Actions { get; set; } = Array.Empty<StepActionDto>();


	}
}

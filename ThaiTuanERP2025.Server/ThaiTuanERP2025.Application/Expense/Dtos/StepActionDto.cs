using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class StepActionDto
	{
		public Guid Id { get; set; }
		public Guid StepInstanceId { get; set; }
		public Guid ActorUserId { get; set; }
		public string Action { get; set; } = default!;
		public string? Comment { get; set; }
		public DateTime OccuredAt {  get; set; }
		public IReadOnlyList<Guid>? AttachmentFileIds { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalAction : AuditableEntity
	{
		public Guid StepInstanceId {  get; set; }
		public ApprovalStepInstance StepInstance { get; set; } = default!;

		public Guid ActorUserId { get; set; }
		public ApprovalActionType Action {  get; set; }
		public string? Comment { get; set; }	
		public string? AttachmentFileIdsJson { get; set; } // StoredFileId[]

		public DateTime OccuredAt { get; set; } = DateTime.UtcNow;

	}
}

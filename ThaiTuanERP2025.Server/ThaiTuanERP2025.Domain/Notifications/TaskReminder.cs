using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Notifications
{
	public class TaskReminder : AuditableEntity
	{
		public Guid UserId { get; private set; }
		public Guid WorkflowInstanceId { get; private set; }
		public Guid StepInstanceId { get; private set; }

		public string Title { get; private set; } = string.Empty;   // ví dụ: "Cần duyệt bước 'Trưởng phòng'"
		public string Message { get; private set; } = string.Empty; // ví dụ: "Đơn chi #EP-000123"
		public DateTime DueAt { get; private set; }                  // ActivatedAt + SlaHours
		public bool IsResolved { get; private set; }                 // đã approve / đã hết hạn / bị dismiss
		public string? ResolvedReason { get; private set; }          // "Approved" | "Expired" | "Dismissed"
		public DateTime? ResolvedAt { get; private set; }

		public Guid DocumentId { get; private set; }      // Ví dụ: Id của ExpensePayment
		public string DocumentType { get; private set; } = string.Empty;

		private TaskReminder() { }

		public static TaskReminder Create(
			Guid userId,
			Guid workflowInstanceId,
			Guid stepInstanceId,
			string title,
			string message,
			Guid documentId,
			string documentType,
			DateTime dueAt
		)
		{
			return new TaskReminder
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				WorkflowInstanceId = workflowInstanceId,
				StepInstanceId = stepInstanceId,
				Title = title?.Trim() ?? string.Empty,
				Message = message?.Trim() ?? string.Empty,
				DocumentId = documentId,
				DocumentType = documentType,
				DueAt = dueAt,
				IsResolved = false
			};
		}

		public void Resolve(string reason)
		{
			if (IsResolved) return;
			IsResolved = true;
			ResolvedReason = reason;
			ResolvedAt = DateTime.UtcNow;
		}
	}
}

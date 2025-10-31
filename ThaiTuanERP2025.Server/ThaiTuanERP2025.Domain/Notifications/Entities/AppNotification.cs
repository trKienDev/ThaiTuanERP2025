using ThaiTuanERP2025.Domain.Common.Entities;

namespace ThaiTuanERP2025.Domain.Notifications.Entities
{
	public class AppNotification : AuditableEntity
	{
		public Guid UserId { get; private set; }
		public string Title { get; private set; } = string.Empty;
		public string Message { get; private set; } = string.Empty;
		public string Link { get; private set; } = string.Empty;
		public bool IsRead { get; private set; }

		public string DocumentType { get; private set; } = string.Empty;
		public Guid DocumentId { get; private set; }
		public Guid? WorkflowInstanceId { get; private set; }
		public Guid? WorkflowStepInstanceId { get; private set; }

		private AppNotification() { } // EF

		public static AppNotification Create(
			Guid userId,
			string title,
			string message,
			string link,
			string documentType,
			Guid documentId,
			Guid? workflowInstanceId,
			Guid? workflowStepInstanceId
		)
		{
			return new AppNotification
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				Title = title?.Trim() ?? string.Empty,
				Message = message?.Trim() ?? string.Empty,
				Link = link?.Trim() ?? string.Empty,
				IsRead = false,
				DocumentType = documentType ?? string.Empty,
				DocumentId = documentId,
				WorkflowInstanceId = workflowInstanceId,
				WorkflowStepInstanceId = workflowStepInstanceId
			};
		}

		public void MarkRead() => IsRead = true;
	}
}

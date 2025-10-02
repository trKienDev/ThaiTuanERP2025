using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Notifications.Dtos;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Notifications;

namespace ThaiTuanERP2025.Infrastructure.Notifications.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRealtimeNotifier _realtime;
		public NotificationService(IUnitOfWork unitOfWork, IRealtimeNotifier realtime)
		{
			_unitOfWork = unitOfWork;
			_realtime = realtime;
		}

		public async Task NotifyStepActivatedAsync(ApprovalWorkflowInstance instance, ApprovalStepInstance step, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken)
		{
			if (targetUserIds == null || targetUserIds.Count == 0) return;

			// 1 ) Tạo các notification mới (lọc duplicate như hiện tại của bạn)
			var notifications = new List<AppNotification>(); // sau

			foreach (var uid in targetUserIds.Distinct())
			{
				// Idempotency nhẹ: không tạo trùng cùng step cho user
				var existed = await _unitOfWork.Notifications.AnyAsync(n =>
					n.UserId == uid
					&& n.DocumentType == instance.DocumentType
					&& n.DocumentId == instance.DocumentId
					&& n.WorkflowStepInstanceId == step.Id
				);

				if (existed) continue;

				notifications.Add(AppNotification.Create(
					userId: uid,
					title: $"Yêu cầu duyệt: {step.Name}",
					message: BuildMessage(instance, step),
					link: BuildLink(instance),
					documentType: instance.DocumentType,
					documentId: instance.DocumentId,
					workflowInstanceId: instance.Id,
					workflowStepInstanceId: step.Id
				));
			}

			if (notifications.Count == 0) return;

			// 2 ) Lưu vào DB
			await _unitOfWork.Notifications.AddRangeAsync(notifications, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// 3 ) Push SignalR (real-time)
			// Map sang DTO nhẹ để gửi ra client
			var payloads = notifications.Select(n => new AppNotificationDto(
				Id: n.Id,
				Title: n.Title,
				Message: n.Message,
				Link: n.Link ?? "/",
				CreatedAt: n.CreatedDate
			)).Cast<object>().ToList();

			await _realtime.NotifyStepActivatedAsync(targetUserIds, payloads, cancellationToken);
		}

		private static string BuildLink(ApprovalWorkflowInstance i)
			=> i.DocumentType == "ExpensePayment" ? $"/expense/payments/{i.DocumentId}" : "/";

		private static string BuildMessage(ApprovalWorkflowInstance i, ApprovalStepInstance s)
			=> $"Tài liệu: {i.DocumentType} • Bước: {s.Name}" + (s.DueAt.HasValue ? $" • Hạn SLA: {s.DueAt.Value:HH:mm dd/MM}" : string.Empty);
	}
}

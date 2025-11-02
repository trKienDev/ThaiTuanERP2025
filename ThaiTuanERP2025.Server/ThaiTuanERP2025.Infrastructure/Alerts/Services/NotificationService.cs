using ThaiTuanERP2025.Application.Alerts.Notifications;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Domain.Alerts.Entities;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Alerts.Services
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

		public async Task NotifyStepActivatedAsync(ExpenseWorkflowInstance instance, ExpenseStepInstance step, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken)
		{
			if (targetUserIds == null || targetUserIds.Count == 0) return;

			// 1 ) Tạo các notification mới (lọc duplicate như hiện tại của bạn)
			var notifications = new List<AppNotification>(); // sau

			foreach (var uid in targetUserIds.Distinct())
			{
				// Idempotency nhẹ: không tạo trùng cùng step cho user
				var existed = await _unitOfWork.Notifications.ExistAsync(n =>
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
				CreatedAt: n.CreatedDate,
				IsRead: n.IsRead
			)).Cast<object>().ToList();

			await _realtime.NotifyStepActivatedAsync(targetUserIds, payloads, cancellationToken);
		}

		public async Task NotifyWorkflowRejectedAsync(ExpenseWorkflowInstance workflow, ExpenseStepInstance step, string docName, Guid docId, string docType, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken) { 
			if (targetUserIds == null || targetUserIds.Count == 0) return;

			var title = $"{docName} đã bị từ chối";
			var message = $"Chứng từ {docName} đã bị từ chối ở bước {step.Name}";
			var link = BuildLink(workflow); // tái dùng BuildLink sẵn có

			var notifications = targetUserIds.Distinct().Select(uid =>
				AppNotification.Create(
					userId: uid,
					title: title,
					message: message,
					link: link,
					documentType: docType,
					documentId: docId,
					workflowInstanceId: workflow.Id,
					workflowStepInstanceId: null // kết thúc workflow: không gắn step cụ thể
				)
			).ToList();

			await _unitOfWork.Notifications.AddRangeAsync(notifications, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// push realtime (dùng cùng kênh "ReceiveNotification")
			var payloads = notifications.Select(n => new {
				id = n.Id,
				title = n.Title,
				message = n.Message,
				link = n.Link,
				createdAt = n.CreatedDate,
				isRead = n.IsRead
			}).ToList();
			await _realtime.NotifyStepActivatedAsync(targetUserIds, payloads, cancellationToken);
		}

		public async Task NotifyWorkflowApprovedAsync(ExpenseWorkflowInstance workflow, ExpenseStepInstance step, IReadOnlyCollection<Guid> targetUserIds, string approver, string docName, Guid documentId, string documentType, CancellationToken cancellationToken = default) {
			if (targetUserIds == null || targetUserIds.Count == 0) return;

			var title = $"{docName} đã được chấp thuận";
			var message = $"Chứng từ {docName} đã được {approver} duyệt";
			var link = BuildLink(workflow);

			var notifications = targetUserIds.Distinct().Select(uid =>
				AppNotification.Create(
					userId: uid,
					title: title,
					message: message,
					link: link,
					documentType: documentType,
					documentId: documentId,
					workflowInstanceId: workflow.Id,
					workflowStepInstanceId: null // kết thúc workflow: không gắn step cụ thể
				)
			).ToList();

			await _unitOfWork.Notifications.AddRangeAsync(notifications, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			// push realtime (dùng cùng kênh "ReceiveNotification")
			var payloads = notifications.Select(n => new {
				id = n.Id,
				title = n.Title,
				message = n.Message,
				link = n.Link,
				createdAt = n.CreatedDate,
				isRead = n.IsRead
			}).ToList();
			await _realtime.NotifyStepActivatedAsync(targetUserIds, payloads, cancellationToken);
		}

		private static string BuildLink(ExpenseWorkflowInstance i)
			=> i.DocumentType == "ExpensePayment" ? $"/expense/payments/{i.DocumentId}" : "/";

		private static string BuildMessage(ExpenseWorkflowInstance i, ExpenseStepInstance s)
			=> $"Tài liệu: {i.DocumentType} • Bước: {s.Name}" + (s.DueAt.HasValue ? $" • Hạn SLA: {s.DueAt.Value:HH:mm dd/MM}" : string.Empty);
	}
}

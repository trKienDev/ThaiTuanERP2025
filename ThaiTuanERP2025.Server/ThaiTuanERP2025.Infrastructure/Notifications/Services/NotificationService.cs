using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Notifications;

namespace ThaiTuanERP2025.Infrastructure.Notifications.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IUnitOfWork _unitOfWork;
		public NotificationService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task NotifyStepActivatedAsync(ApprovalWorkflowInstance instance, ApprovalStepInstance step, IReadOnlyCollection<Guid> targetUserIds, CancellationToken cancellationToken)
		{
			if (targetUserIds == null || targetUserIds.Count == 0) return;

			var notis = new List<AppNotification>(targetUserIds.Count);

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

				notis.Add(AppNotification.Create(
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

			if (notis.Count > 0)
			{
				await _unitOfWork.Notifications.AddRangeAsync(notis, cancellationToken);
				await _unitOfWork.SaveChangesAsync(cancellationToken);
				// (Optional) Đẩy SignalR tại đây nếu bạn đã có Hub
			}
		}

		private static string BuildLink(ApprovalWorkflowInstance i)
			=> i.DocumentType == "ExpensePayment" ? $"/expense/payments/{i.DocumentId}" : "/";

		private static string BuildMessage(ApprovalWorkflowInstance i, ApprovalStepInstance s)
			=> $"Tài liệu: {i.DocumentType} • Bước: {s.Name}" + (s.DueAt.HasValue ? $" • Hạn SLA: {s.DueAt.Value:HH:mm dd/MM}" : string.Empty);
	}
}

using ThaiTuanERP2025.Application.Common.Utils;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Services
{
	public class ApprovalStepService : IApprovalStepService
	{
		private readonly INotificationService _notificationService;
		private readonly ITaskReminderService _taskReminderService;
		public ApprovalStepService(INotificationService notificationService, ITaskReminderService taskReminderService)
		{
			_notificationService = notificationService;
			_taskReminderService = taskReminderService;
		}

		public async Task PublishAsync(ApprovalWorkflowInstance workflowInstance, ApprovalStepInstance stepInstance, string docName, Guid docId, string docType, CancellationToken cancellationToken)
		{
			var targets = ResolveTargetsForNotification(stepInstance);
			if (targets.Count > 0)
				await _notificationService.NotifyStepActivatedAsync(workflowInstance, stepInstance, targets, cancellationToken);

			var approverIds = JsonUtils.ParseGuidArray(stepInstance.ResolvedApproverCandidatesJson);
			await _taskReminderService.CreateForStepActivationAsync(
				stepInstance.Id, workflowInstance.Id, approverIds,
				title: $"Cần duyệt bước \"{stepInstance.Name}\"",
				message: $"Chứng từ {docName}",
				documentId: docId,
				documentType: docType,
				dueAt: stepInstance.DueAt!.Value,
				cancellationToken
			);
		}

		public static IReadOnlyCollection<Guid> ResolveTargetsForNotification(ApprovalStepInstance step)
		{
			// 1) Nếu đã chọn SelectedApproverId → notify đúng 1 người
			if (step.SelectedApproverId.HasValue && step.SelectedApproverId.Value != Guid.Empty)
				return new[] { step.SelectedApproverId.Value };

			// 2) Ưu tiên danh sách resolved candidates
			Guid[] ids = Array.Empty<Guid>();
			if (!string.IsNullOrWhiteSpace(step.ResolvedApproverCandidatesJson))
			{
				try
				{
					ids = System.Text.Json.JsonSerializer
					    .Deserialize<List<Guid>>(step.ResolvedApproverCandidatesJson!)?
					    .ToArray() ?? Array.Empty<Guid>();
				}
				catch { /* ignore */ }
			}

			// 3) Fallback về DefaultApproverId
			if (ids.Length == 0 && step.DefaultApproverId.HasValue && step.DefaultApproverId.Value != Guid.Empty)
				return new[] { step.DefaultApproverId.Value };

			return ids;
		}
	}
}

using MediatR;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Services;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Events;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.EventHandlers
{
	public sealed class ExpenseStepInstanceActivatedEventHandler : INotificationHandler<ExpenseStepInstanceActivatedEvent>
	{
		private readonly INotificationService _notificationService;
		private readonly IReminderService _reminderService;
		private readonly IExpenseWorkflowInstanceReadRepository _workflowInstanceRepo;
		private readonly IDocumentResolver _documentResolver;
		public ExpenseStepInstanceActivatedEventHandler(
			INotificationService notificationService, IReminderService reminderService,
			IExpenseWorkflowInstanceReadRepository workflowInstanceRepo, IDocumentResolver documentResolver
		) {
			_notificationService = notificationService;
			_reminderService = reminderService;
			_workflowInstanceRepo = workflowInstanceRepo;
			_documentResolver = documentResolver;
		}

		public async Task Handle(ExpenseStepInstanceActivatedEvent domainEvent, CancellationToken cancellationToken) {
			var stepInstance = domainEvent.StepInstance;
			if (stepInstance.SelectedApproverId is null)
				throw new DomainException("Step hiện tại chưa có người duyệt (SelectedApproverId = null), không thể tạo reminder.");

			if(stepInstance.DueAt is null)
				throw new DomainException("Step hiện tại chưa có thời hạn, không thể tạo reminder.");

			var workflowInstanceDto = await _workflowInstanceRepo.GetByIdProjectedAsync(stepInstance.WorkflowInstanceId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy luồng duyệt");

			var expensePaymentName = await _documentResolver.GetDocumentNameAsync(
				workflowInstanceDto.DocumentType,
				workflowInstanceDto.Id,
				cancellationToken
			);
			var expensePaymentCreatorId = await _documentResolver.GetDocumentCreatorIdAsync(
				workflowInstanceDto.DocumentType,
				workflowInstanceDto.Id,
				cancellationToken
			);

			var message = $"Thanh toán { expensePaymentName } đang chờ bạn duyệt";

			// Set reminder for approver
			await _reminderService.ScheduleReminderAsync(
				userId: stepInstance.SelectedApproverId.Value,
				subject: $"Duyệt thanh toán { expensePaymentName }",
				message: message,
				slaHours: stepInstance.SlaHours,
				dueAt: stepInstance.DueAt.Value,
				linkType: LinkType.ExpensePaymentApprove,
				targetId: workflowInstanceDto.DocumentId,
				cancellationToken
			);

			// send notification for approver
			await _notificationService.SendAsync(
				senderId: expensePaymentCreatorId,
				receiverId: stepInstance.SelectedApproverId.Value,
				title: $"Duyệt thanh toán {expensePaymentName}",
				message: message,
				linkType: LinkType.ExpensePaymentApprove,
				targetId: workflowInstanceDto.DocumentId,
				type: NotificationType.Task,
				cancellationToken
			);
		}
	}
}

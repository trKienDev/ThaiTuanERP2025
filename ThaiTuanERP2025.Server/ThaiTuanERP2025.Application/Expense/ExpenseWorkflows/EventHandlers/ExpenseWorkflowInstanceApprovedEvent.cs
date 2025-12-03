using MediatR;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Events;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.EventHandlers
{
	public sealed class ExpenseWorkflowInstanceApprovedEventHandler : INotificationHandler<ExpenseWorkflowInstanceApprovedEvent>
	{
		private readonly IUnitOfWork _uow;
		public ExpenseWorkflowInstanceApprovedEventHandler(
			INotificationService notificaiton, IReminderService reminder, IUnitOfWork uow	
		)
		{
			_uow = uow;
		}

		public async Task Handle(ExpenseWorkflowInstanceApprovedEvent domainEvent, CancellationToken cancellationToken)
		{
			if (domainEvent.DocumentType != Domain.Shared.Enums.DocumentType.ExpensePayment)
				return;

			var payment = await _uow.ExpensePayments.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == domainEvent.DocumentId),
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy thanh toán hợp lệ");

			payment.MarkApproved();

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}
	}
}

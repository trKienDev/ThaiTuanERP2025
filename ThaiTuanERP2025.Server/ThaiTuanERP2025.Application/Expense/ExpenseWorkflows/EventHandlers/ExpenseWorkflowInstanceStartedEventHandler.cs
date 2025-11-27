using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Events;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.EventHandlers
{
	public sealed class ExpenseWorkflowInstanceStartedEventHandler : INotificationHandler<ExpenseWorkflowInstanceStartedEvent>
	{
		private readonly IUnitOfWork _uow;
		public ExpenseWorkflowInstanceStartedEventHandler(IUnitOfWork uow) {
			_uow = uow;
		}

		public async Task Handle(ExpenseWorkflowInstanceStartedEvent domainEvent, CancellationToken cancellationToken)
		{
			var instance = domainEvent.WorkflowInstance;

			var workflow = await _uow.ExpenseWorkflowInstances.SingleOrDefaultIncludingAsync(
				predicate: x => x.Id == instance.Id,
				asNoTracking: false,
				includes: x => x.Steps,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy luồng duyệt của chi phí");

			var startStep = workflow.Steps.FirstOrDefault(s => s.Order == workflow.CurrentStepOrder)
				?? throw new DomainException("Không tìm thấy step khởi động");

			startStep.Activate();
			await _uow.SaveChangesAsync(cancellationToken);
		}
	}
}

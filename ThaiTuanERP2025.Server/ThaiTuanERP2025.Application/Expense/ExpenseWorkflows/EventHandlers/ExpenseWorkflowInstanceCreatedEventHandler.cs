using MediatR;
using ThaiTuanERP2025.Domain.Expense.Events;
using ThaiTuanERP2025.Domain.Shared.Repositories;
namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.EventHandlers
{
	public sealed class ExpenseWorkflowInstanceCreatedEventHandler : INotificationHandler<ExpenseWorkflowInstanceCreatedEvent>
	{
		private readonly IUnitOfWork _uow;
		public ExpenseWorkflowInstanceCreatedEventHandler(IUnitOfWork uow, IServiceProvider sp) {
			Console.WriteLine(">> Handler Constructed  [HashCode: " + GetHashCode() + "] [Provider: " + sp.GetHashCode() + "]");
			_uow = uow;
		}

		public async Task Handle(ExpenseWorkflowInstanceCreatedEvent domainEvent, CancellationToken cancellationToken)
		{
			domainEvent.WorkflowInstance.Start();
			await _uow.SaveChangesAsync(cancellationToken);
		}
	}
}

using MediatR;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Factories;
using ThaiTuanERP2025.Domain.Expense.Events;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.EventHandlers
{
	public sealed class ExpensePaymentCreatedEventHandler : INotificationHandler<ExpensePaymentCreatedEvent>
	{
		private readonly IExpenseWorkflowFactory _expenseWorkflowFactory;
		private readonly IUnitOfWork _uow;
		public ExpensePaymentCreatedEventHandler(IUnitOfWork uow, IExpenseWorkflowFactory expenseWorkflowFactory)
		{
			_uow = uow;
			_expenseWorkflowFactory = expenseWorkflowFactory;
		}

		public async Task Handle(ExpensePaymentCreatedEvent domainEvent, CancellationToken cancellationToken)
		{
			var createdPayment = domainEvent.ExpensePayment;
			
			// === Create workflow instance ===
			var workflowInstance = await _expenseWorkflowFactory.CreateForExpensePaymentAsync(createdPayment, cancellationToken);
			createdPayment.LinkWorkflowInstance(workflowInstance);
			await _uow.ExpenseWorkflowInstances.AddAsync(workflowInstance, cancellationToken);

			await _uow.SaveChangesAsync(cancellationToken);
		}
	}
}

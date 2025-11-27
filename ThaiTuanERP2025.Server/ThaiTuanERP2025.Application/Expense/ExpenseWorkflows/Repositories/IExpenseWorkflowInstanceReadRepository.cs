using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Repositories
{
	public interface IExpenseWorkflowInstanceReadRepository : IBaseReadRepository<ExpenseWorkflowInstance, ExpenseWorkflowInstanceDto>
	{
	}
}

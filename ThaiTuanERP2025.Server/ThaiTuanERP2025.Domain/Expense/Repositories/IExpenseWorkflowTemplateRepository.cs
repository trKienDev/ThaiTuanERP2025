using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IExpenseWorkflowTemplateRepository : IBaseWriteRepository<ExpenseWorkflowTemplate>
	{
		Task<bool> ExistsActiveForScopeAsync(CancellationToken cancellationToken = default);
		Task<List<ExpenseWorkflowTemplate>> ListByFilterAsync(bool? isActive, CancellationToken cancellationToken = default);
	}
}

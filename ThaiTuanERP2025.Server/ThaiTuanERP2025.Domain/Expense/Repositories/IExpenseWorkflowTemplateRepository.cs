using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IExpenseWorkflowTemplateRepository : IBaseWriteRepository<ExpenseWorkflowTemplate>
	{
		Task<bool> ExistsActiveForScopeAsync(string documentType, CancellationToken cancellationToken = default);
		Task<List<ExpenseWorkflowTemplate>> ListByFilterAsync(string? documentType, bool? isActive, CancellationToken cancellationToken = default);
	}
}

using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IExpenseStepTemplateRepository : IBaseRepository<ExpenseStepTemplate>
	{
		Task<bool> ExistOrderAsync(Guid workflowTemplateId, int order, Guid? excludeId = null, CancellationToken cancellationToken = default);
	}
}

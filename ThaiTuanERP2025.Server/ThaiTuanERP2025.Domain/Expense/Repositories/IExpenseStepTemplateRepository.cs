using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IExpenseStepTemplateRepository : IBaseWriteRepository<ExpenseStepTemplate>
	{
		Task<bool> ExistOrderAsync(Guid workflowTemplateId, int order, Guid? excludeId = null, CancellationToken cancellationToken = default);
	}
}

using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IExpenseStepInstanceRepository : IBaseWriteRepository<ExpenseStepInstance>
	{
		Task<ExpenseStepInstance?> GetByIdWithWorkflowAsync(Guid stepId, CancellationToken cancellationToken = default);
		Task<ExpenseStepInstance?> GetCurrentStepAsync(Guid workflowId, CancellationToken cancellationToken = default);
	}
}

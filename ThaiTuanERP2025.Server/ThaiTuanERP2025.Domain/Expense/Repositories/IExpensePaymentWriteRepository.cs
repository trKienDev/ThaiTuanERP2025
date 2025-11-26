using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IExpensePaymentWriteRepository : IBaseWriteRepository<ExpensePayment>
	{
		Task<ExpenseWorkflowInstance?> GetWorkflowInstanceAsync(Guid documentId, CancellationToken cancellationToken = default);
	}
}

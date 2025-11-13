using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IExpensePaymentRepository : IBaseWriteRepository<ExpensePayment>
	{
		Task<ExpensePayment?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<ExpenseWorkflowInstance?> GetWorkflowInstanceAsync(Guid documentId, CancellationToken cancellationToken = default);
	}
}

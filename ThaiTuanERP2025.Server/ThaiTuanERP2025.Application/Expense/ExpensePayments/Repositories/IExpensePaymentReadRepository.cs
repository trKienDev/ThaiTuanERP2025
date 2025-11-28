using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories
{
	public interface IExpensePaymentReadRepository : IBaseReadRepository<ExpensePayment, ExpensePaymentDto>
	{
		Task<string?> GetNameAsync(Guid expensePaymentId, CancellationToken cancellationToken = default);
		Task<Guid> GetCreatorIdAsync(Guid expensePaymentId, CancellationToken cancellationToken = default);
		Task<ExpensePaymentLookupDto?> GetLookupById(Guid id, CancellationToken cancellationToken = default);
	}
}

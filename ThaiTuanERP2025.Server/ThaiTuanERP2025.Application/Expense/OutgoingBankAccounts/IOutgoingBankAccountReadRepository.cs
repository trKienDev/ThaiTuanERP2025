using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts
{
	public interface IOutgoingBankAccountReadRepository : IBaseReadRepository<OutgoingBankAccount, OutgoingBankAccountDto>
	{
	}
}

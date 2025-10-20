using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.BankAccounts.GetUserBankAccount
{
	public sealed record GetUserBankAccountQuery(Guid Userid) : IRequest<BankAccountDto>;
}

using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.BankAccounts.GetBankAccountById
{
	public sealed record GetBankAccountByIdQuery(Guid Id) : IRequest<BankAccountDto>;
}

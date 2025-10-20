using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.OutgoingBankAccounts.GetAllOutgoingBankAccount
{
	public sealed record GetAllOutgoingBankAccountQuery() : IRequest<IReadOnlyList<OutgoingBankAccountDto>>;
}

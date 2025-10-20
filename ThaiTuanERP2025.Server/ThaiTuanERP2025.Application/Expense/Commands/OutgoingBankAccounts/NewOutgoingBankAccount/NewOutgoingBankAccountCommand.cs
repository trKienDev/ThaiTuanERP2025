using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.OutgoingBankAccounts.NewOutgoingBankAccount
{
	public sealed record NewOutgoingBankAccountCommand(OutgoingBankAccountRequest Request) : IRequest<Unit>;
}

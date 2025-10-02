using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePayments
{
	public sealed record CreateExpensePaymentCommand(
		ExpensePaymentRequest Request
	) : IRequest<Guid>;
}

using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePaymentComment
{
	public sealed record CreateExpensePaymentCommentCommand (
		ExpensePaymentCommentRequest Request
	) : IRequest<ExpensePaymentCommentDto>;
}

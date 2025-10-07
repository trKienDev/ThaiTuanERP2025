using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.Expense.GetCommentsByExpensePaymentId
{
	public sealed record GetCommentsByExpensePaymentIdQuery(
		Guid ExpensePaymentId
	) : IRequest<IReadOnlyList<ExpensePaymentCommentDto>>;
}

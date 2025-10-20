using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ExpensePayment.GetFollowingExpensePayment
{
	public sealed record GetFollowingExpensePaymentQuery : IRequest<IReadOnlyCollection<ExpensePaymentSummaryDto>>
	{
	}
}

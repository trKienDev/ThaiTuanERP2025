using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ExpensePayment.GetFollowingExpensePayment
{
	public sealed record GetFollowingExpensePaymentQuery(
		int Page = 1,
		int PageSize = 20,
		DateTime? UpdatedAfter = null
	) : IRequest<IReadOnlyCollection<ExpensePaymentSummaryDto>>;
}

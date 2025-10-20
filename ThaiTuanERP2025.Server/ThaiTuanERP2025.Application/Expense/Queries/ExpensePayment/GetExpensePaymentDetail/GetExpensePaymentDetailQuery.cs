using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ExpensePayment.GetExpensePaymentDetail
{
	public sealed record GetExpensePaymentDetailQuery(Guid Id) : IRequest<ExpensePaymentDetailDto>;
}

using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.OutgoingPayments.GetFollowingOutgoingPayments
{
	public sealed record GetFollowingOutgoingPaymentsQuery : IRequest<IReadOnlyCollection<OutgoingPaymentSummaryDto>>
	{
	}
}

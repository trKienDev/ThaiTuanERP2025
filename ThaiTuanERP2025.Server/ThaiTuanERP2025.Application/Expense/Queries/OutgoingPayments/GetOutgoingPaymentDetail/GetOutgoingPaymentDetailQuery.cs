using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.OutgoingPayments.GetOutgoingPaymentDetail
{
	public sealed record GetOutgoingPaymentDetailQuery(Guid Id) : IRequest<OutgoingPaymentDetailDto>;
}

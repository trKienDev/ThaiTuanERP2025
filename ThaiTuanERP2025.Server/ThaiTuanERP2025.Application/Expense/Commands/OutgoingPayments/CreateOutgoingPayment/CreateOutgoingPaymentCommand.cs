using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.CreateOutgoingPayment
{
	public sealed record CreateOutgoingPaymentCommand(OutgoingPaymentRequest Request) : IRequest<Unit>;
}

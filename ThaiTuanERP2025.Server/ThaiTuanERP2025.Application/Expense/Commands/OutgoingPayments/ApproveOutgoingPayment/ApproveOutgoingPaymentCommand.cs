using MediatR;

namespace ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.ApproveOutgoingPayment
{
	public sealed record ApproveOutgoingPaymentCommand(Guid Id) : IRequest<Unit> {}
}

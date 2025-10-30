using MediatR;

namespace ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.MarkOutgoingPaymentCreated
{
	public sealed record MarkOutgoingPaymentCreatedCommand(Guid Id) : IRequest<Unit> { }
}

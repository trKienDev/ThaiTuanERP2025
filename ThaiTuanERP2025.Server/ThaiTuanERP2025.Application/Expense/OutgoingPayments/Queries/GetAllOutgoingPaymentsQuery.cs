using MediatR;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Queries
{
        public sealed record GetAllOutgoingPaymentsQuery() : IRequest<IReadOnlyList<OutgoingPaymentDto>>;

        public sealed class  GetAllOutoingPaymentsQueryHandler : IRequestHandler<GetAllOutgoingPaymentsQuery, IReadOnlyList<OutgoingPaymentDto>>
        {
                private readonly IOutgoingPaymentReadRepository _outgoingPaymentRepo;
                public  GetAllOutoingPaymentsQueryHandler(IOutgoingPaymentReadRepository outgoingPaymentRepo)
                {
                        _outgoingPaymentRepo = outgoingPaymentRepo;
                }

                public async Task<IReadOnlyList<OutgoingPaymentDto>> Handle(GetAllOutgoingPaymentsQuery request, CancellationToken cancellationToken)
                {
                        return await _outgoingPaymentRepo.GetAllAsync(cancellationToken: cancellationToken);
                }
        }
}

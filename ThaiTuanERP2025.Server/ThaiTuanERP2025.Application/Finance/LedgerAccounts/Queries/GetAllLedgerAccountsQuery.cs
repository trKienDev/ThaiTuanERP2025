using MediatR;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts.Queries
{
        public sealed record GetAllLedgerAccountsQuery() : IRequest<IReadOnlyList<LedgerAccountDto>>;

        public sealed class GetAllLedgerAccountsQueryHandler : IRequestHandler<GetAllLedgerAccountsQuery, IReadOnlyList<LedgerAccountDto>>
        {
                private readonly ILedgerAccountReadRepository _ledgerAccountRepo;
                public GetAllLedgerAccountsQueryHandler(ILedgerAccountReadRepository ledgerAccountRepo)
                {
                        _ledgerAccountRepo = ledgerAccountRepo;
                }
                        
                public async Task<IReadOnlyList<LedgerAccountDto>> Handle(GetAllLedgerAccountsQuery query, CancellationToken cancellationToken)
                {
                        return await _ledgerAccountRepo.GetAllActiveAsync(cancellationToken);
                }
        }
}

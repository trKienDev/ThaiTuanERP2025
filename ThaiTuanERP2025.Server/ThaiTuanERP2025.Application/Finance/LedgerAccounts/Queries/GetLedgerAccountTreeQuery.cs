using MediatR;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts.Queries
{
	public sealed record GetLedgerAccountTreeQuery : IRequest<IReadOnlyList<LedgerAccountTreeDto>>;

	public sealed class GetLedgerAccountTreeQueryHandler : IRequestHandler<GetLedgerAccountTreeQuery, IReadOnlyList<LedgerAccountTreeDto>>
	{
		private readonly ILedgerAccountReadRepository _ledgerAccountRepo;
		public GetLedgerAccountTreeQueryHandler(ILedgerAccountReadRepository ledgerAccountRepo)
		{
			_ledgerAccountRepo = ledgerAccountRepo;	
		}

		public async Task<IReadOnlyList<LedgerAccountTreeDto>> Handle(GetLedgerAccountTreeQuery query, CancellationToken cancellationToken)
		{
			return await _ledgerAccountRepo.GetTreeAsync(cancellationToken);
		}
	}
}

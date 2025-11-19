using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Queries
{
	public sealed record GetAllLedgerAccountTypesQuery : IRequest<IReadOnlyList<LedgerAccountTypeDto>>;

	public sealed class GetAllLedgerAccountTypesQueryHandler : IRequestHandler<GetAllLedgerAccountTypesQuery, IReadOnlyList<LedgerAccountTypeDto>>
	{
		private readonly ILedgerAccountTypeReadRepository _LATypeRepo;
		public GetAllLedgerAccountTypesQueryHandler(ILedgerAccountTypeReadRepository LATypeRepo)
		{
			_LATypeRepo = LATypeRepo;
		}

		public async Task<IReadOnlyList<LedgerAccountTypeDto>> Handle(GetAllLedgerAccountTypesQuery query, CancellationToken cancellationToken)
		{
			return await _LATypeRepo.GetAllAsync(
				x => x.IsActive && !x.IsDeleted, 
				cancellationToken: cancellationToken
			);
		}
	}
}

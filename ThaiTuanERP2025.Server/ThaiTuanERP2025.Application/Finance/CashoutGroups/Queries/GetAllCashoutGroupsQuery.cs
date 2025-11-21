using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Finance.CashoutCodes;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;

namespace ThaiTuanERP2025.Application.Finance.CashoutGroups.Queries
{
	public sealed record GetAllCashoutGroupsQuery : IRequest<IReadOnlyList<CashoutGroupDto>>;

	public sealed class GetAllCashoutGroupsQueryHandler : IRequestHandler<GetAllCashoutGroupsQuery, IReadOnlyList<CashoutGroupDto>>
	{
		private readonly ICashoutGroupReadRepository _cashoutGroupRepo;
		public GetAllCashoutGroupsQueryHandler(ICashoutGroupReadRepository cashoutGroupRepo)
		{
			_cashoutGroupRepo = cashoutGroupRepo;	
		}

		public async Task<IReadOnlyList<CashoutGroupDto>> Handle(GetAllCashoutGroupsQuery query, CancellationToken cancellationToken) {
			return await _cashoutGroupRepo.GetAllAsync(
				q => q.IsActive,
				cancellationToken: cancellationToken
			);
		}
	}
}

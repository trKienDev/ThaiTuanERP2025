using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;

namespace ThaiTuanERP2025.Application.Finance.CashoutGroups.Queries
{
	public sealed record GetCashoutGroupsTreeQuery() : IRequest<IReadOnlyList<CashoutGroupTreeDto>>;

	public sealed class GetCashoutGroupsTreeQueryHandler : IRequestHandler<GetCashoutGroupsTreeQuery, IReadOnlyList<CashoutGroupTreeDto>>
	{
		private readonly ICashoutGroupReadRepository _cashoutGroupRepo;
		private readonly IMapper _mapper;
		public GetCashoutGroupsTreeQueryHandler(ICashoutGroupReadRepository cashoutGroupRepo, IMapper mapper)
		{
			_cashoutGroupRepo = cashoutGroupRepo;
			_mapper = mapper;	
		}

		public async Task<IReadOnlyList<CashoutGroupTreeDto>> Handle(GetCashoutGroupsTreeQuery query, CancellationToken cancellationToken) {
			return await _cashoutGroupRepo.ListProjectedAsync(
				q => q.Where(x => x.IsActive)
					.OrderBy(x => x.Path)
					.ProjectTo<CashoutGroupTreeDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

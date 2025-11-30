using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries
{
	public sealed record GetAllBudgetPeriodsQuery() : IRequest<IReadOnlyList<BudgetPeriodLookupDto>>;

	public sealed class GetAllBudgetPeriodsQueryHandler : IRequestHandler<GetAllBudgetPeriodsQuery, IReadOnlyList<BudgetPeriodLookupDto>>
	{
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		private readonly IMapper _mapper;
		public GetAllBudgetPeriodsQueryHandler(IBudgetPeriodReadRepository budgetPeriodRepo, IMapper mapper)
		{
			_budgetPeriodRepo = budgetPeriodRepo;
			_mapper = mapper;
		}
		
		public async Task<IReadOnlyList<BudgetPeriodLookupDto>> Handle(GetAllBudgetPeriodsQuery query, CancellationToken cancellationToken) {
			return await _budgetPeriodRepo.ListProjectedAsync(
				q => q.Where(x => !x.IsDeleted)
					.ProjectTo<BudgetPeriodLookupDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

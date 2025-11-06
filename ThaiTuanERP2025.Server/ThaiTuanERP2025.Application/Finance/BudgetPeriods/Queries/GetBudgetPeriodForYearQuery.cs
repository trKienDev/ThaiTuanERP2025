using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries
{
	public sealed record GetBudgetPeriodsForYearQuery(int Year) : IRequest<IReadOnlyList<BudgetPeriodDto>>;
	public sealed class GetBudgetPeriodsForYearHandler : IRequestHandler<GetBudgetPeriodsForYearQuery, IReadOnlyList<BudgetPeriodDto>>
	{
		private readonly IBudgetPeriodReadRepository _budgetPeriodReadRepo;
		private readonly IMapper _mapper;
		public GetBudgetPeriodsForYearHandler(IBudgetPeriodReadRepository budgetPeriodReadRepo, IMapper mapper)
		{
			_mapper = mapper;
			_budgetPeriodReadRepo = budgetPeriodReadRepo;
		}

		public async Task<IReadOnlyList<BudgetPeriodDto>> Handle(GetBudgetPeriodsForYearQuery query, CancellationToken cancellationToken)
		{
			return await _budgetPeriodReadRepo.ListProjectedAsync(
				q => q.Where(bp => bp.Year == query.Year)
					.OrderBy(bp => bp.Month)
					.ProjectTo<BudgetPeriodDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

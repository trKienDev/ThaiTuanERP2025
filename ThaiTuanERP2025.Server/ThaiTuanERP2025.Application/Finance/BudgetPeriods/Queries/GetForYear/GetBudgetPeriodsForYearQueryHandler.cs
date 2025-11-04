using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Finance.Budgets.DTOs;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries.GetForYear
{
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

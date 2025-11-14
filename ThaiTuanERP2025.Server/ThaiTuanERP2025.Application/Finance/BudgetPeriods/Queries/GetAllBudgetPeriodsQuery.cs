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
			var today = DateTime.UtcNow.Date;
			return await _budgetPeriodRepo.ListProjectedAsync(
				q => q.Where(x =>
					today >= x.StartDate.Date && today <= x.EndDate.Date
				).ProjectTo<BudgetPeriodLookupDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

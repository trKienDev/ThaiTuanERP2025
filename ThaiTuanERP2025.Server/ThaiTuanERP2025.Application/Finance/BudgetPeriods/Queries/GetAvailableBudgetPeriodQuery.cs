using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries
{
	public sealed record GetAvailableBudgetPeriodQuery() : IRequest<IReadOnlyList<BudgetPeriodDto>>;
	public sealed class GetAvaliableBudgetPeriodQueryHandler : IRequestHandler<GetAvailableBudgetPeriodQuery, IReadOnlyList<BudgetPeriodDto>> {
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		private readonly IMapper _mapper;
		public GetAvaliableBudgetPeriodQueryHandler(IBudgetPeriodReadRepository budgetPeriodRepo, IMapper mapper)
		{
			_budgetPeriodRepo = budgetPeriodRepo;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<BudgetPeriodDto>> Handle(GetAvailableBudgetPeriodQuery query, CancellationToken cancellationToken) {
			var now = DateTime.UtcNow;
			return await _budgetPeriodRepo.ListProjectedAsync(
				q => q.Where(bp => !bp.IsDeleted
					&& bp.StartDate <= now && bp.EndDate >= now
				).OrderBy(bp => bp.Month)
				.ProjectTo<BudgetPeriodDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

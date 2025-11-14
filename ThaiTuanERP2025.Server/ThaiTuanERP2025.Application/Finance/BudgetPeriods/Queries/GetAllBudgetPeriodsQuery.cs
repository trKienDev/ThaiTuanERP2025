using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries
{
	public sealed record GetAllBudgetPeriodsQuery() : IRequest<IReadOnlyList<BudgetPeriodDto>>;

	public sealed class GetAllBudgetPeriodsQueryHandler : IRequestHandler<GetAllBudgetPeriodsQuery, IReadOnlyList<BudgetPeriodDto>>
	{
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		public GetAllBudgetPeriodsQueryHandler(IBudgetPeriodReadRepository budgetPeriodRepo)
		{
			_budgetPeriodRepo = budgetPeriodRepo;
		}
		
		public async Task<IReadOnlyList<BudgetPeriodDto>> Handle(GetAllBudgetPeriodsQuery query, CancellationToken cancellationToken) {
			return await _budgetPeriodRepo.GetAllAsync(
				q => !q.IsDeleted,
				cancellationToken: cancellationToken
			);
		}
	}
}

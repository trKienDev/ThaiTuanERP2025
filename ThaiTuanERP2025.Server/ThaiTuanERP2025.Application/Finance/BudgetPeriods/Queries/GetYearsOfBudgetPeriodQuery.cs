using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries
{
	public sealed record GetYearsOfBudgetPeriodQuery() : IRequest<IReadOnlyList<int>>;

	public sealed class GetYearsOfBudgetPeriodQueryHandler : IRequestHandler<GetYearsOfBudgetPeriodQuery, IReadOnlyList<int>>
	{
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		public GetYearsOfBudgetPeriodQueryHandler(IBudgetPeriodReadRepository budgetPeriodRepo)
		{
			_budgetPeriodRepo = budgetPeriodRepo;
		}
		
		public async Task<IReadOnlyList<int>> Handle(GetYearsOfBudgetPeriodQuery query, CancellationToken cancellationToken) {
			var years = await _budgetPeriodRepo.ListProjectedAsync(
				q => q.Select(x => x.Year)    
					.Distinct()            
					.OrderBy(y => y),     
				cancellationToken: cancellationToken
			);

			return years;
		}
	}
}

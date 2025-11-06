using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetGroups.Query
{
	public sealed record GetAllBudgetGroupsQuery() : IRequest<IReadOnlyList<BudgetGroupDto>>;
	public sealed class GetAllBudgetGroupsQueryHandler : IRequestHandler<GetAllBudgetGroupsQuery, IReadOnlyList<BudgetGroupDto>>
	{
		private readonly IBudgetGroupReadRepository _budgetGroupReadRepo;
		private readonly IMapper _mapper;
		public GetAllBudgetGroupsQueryHandler(IBudgetGroupReadRepository budgetGroupReadRepo, IMapper mapper)
		{
			_budgetGroupReadRepo = budgetGroupReadRepo;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<BudgetGroupDto>> Handle(GetAllBudgetGroupsQuery query, CancellationToken cancellationToken)
		{
			return await _budgetGroupReadRepo.ListProjectedAsync(
				q => q.Where(bg => !bg.IsDeleted)
					.OrderBy(bg => bg.Name)
					.ProjectTo<BudgetGroupDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetCodes.Queries
{
	public sealed record GetAllBudgetCodesQuery() : IRequest<IReadOnlyList<BudgetCodeDto>>;

	public sealed class GetAllBudgetCodesQueryHandler : IRequestHandler<GetAllBudgetCodesQuery, IReadOnlyList<BudgetCodeDto>>
	{
		private readonly IBudgetCodeReadRepository _budgetCodeReadRepo;
		private readonly IMapper _mapper;
		public GetAllBudgetCodesQueryHandler(IBudgetCodeReadRepository budgetCodeReadRepo, IMapper mapper)
		{
			_budgetCodeReadRepo = budgetCodeReadRepo;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<BudgetCodeDto>> Handle(GetAllBudgetCodesQuery query, CancellationToken cancellationToken) {
			return await _budgetCodeReadRepo.ListProjectedAsync(
				q => q.Where(bg => !bg.IsDeleted)
				.ProjectTo<BudgetCodeDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

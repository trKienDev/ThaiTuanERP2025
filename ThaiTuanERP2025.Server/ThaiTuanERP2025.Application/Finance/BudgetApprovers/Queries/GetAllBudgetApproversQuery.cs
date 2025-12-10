using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetApprovers.Queries
{
	public sealed record GetAllBudgetApproversQuery() : IRequest<IReadOnlyList<BudgetApproverDto>>;

	public sealed class GetAllBudgetApproversQueryHandler : IRequestHandler<GetAllBudgetApproversQuery, IReadOnlyList<BudgetApproverDto>> {
		private readonly IBudgetApproverReadRepository _budgetApproverRepo;
		private readonly IMapper _mapper;
		public GetAllBudgetApproversQueryHandler(IBudgetApproverReadRepository budgetApproverRepo, IMapper mapper)
		{
			_budgetApproverRepo = budgetApproverRepo;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<BudgetApproverDto>> Handle(GetAllBudgetApproversQuery query, CancellationToken cancellationToken) {
			return await _budgetApproverRepo.ListProjectedAsync(
				q => q.Where(ba => ba.IsActive && !ba.IsDeleted)
					.ProjectTo<BudgetApproverDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}

}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Queries
{
	public sealed record GetFollowingBudgetPlansQuery() : IRequest<IReadOnlyList<BudgetPlanDto>>;

	public sealed class GetFollowingBudgetPlansQueryHandler : IRequestHandler<GetFollowingBudgetPlansQuery, IReadOnlyList<BudgetPlanDto>>
	{
		private readonly IBudgetPlanReadRepository _budgetPlanRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IFollowerReadRepository _followerRepo;
		private readonly IMapper _mapper;
		public GetFollowingBudgetPlansQueryHandler(
			IMapper mapper, IBudgetPlanReadRepository budgetPlanRepo,ICurrentUserService currentUser, IFollowerReadRepository followerRepo
		)
		{
			_budgetPlanRepo = budgetPlanRepo;
			_currentUser = currentUser;
			_followerRepo = followerRepo;	
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<BudgetPlanDto>> Handle(GetFollowingBudgetPlansQuery query, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");

			var following = await _followerRepo.GetAllAsync(
				q => q.UserId == userId && q.SubjectType == SubjectType.BudgetPlan && q.IsActive,
				cancellationToken: cancellationToken
			);

			var subjectIds = following.Select(f => f.SubjectId).ToList();
			if (!subjectIds.Any())
				return Array.Empty<BudgetPlanDto>();

			return await _budgetPlanRepo.ListProjectedAsync(
					q => q.Where(p => subjectIds.Contains(p.Id)
						&& p.IsActive
						&& !p.IsDeleted
				).ProjectTo<BudgetPlanDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Shared;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Queries
{
	public sealed record GetFollowingBudgetPlansByPeriodQuery(Guid PeriodId) : IRequest<IReadOnlyList<BudgetPlanDto>>;

	public sealed class GetFollowingBudgetPlansByPeriodQueryHandler : IRequestHandler<GetFollowingBudgetPlansByPeriodQuery, IReadOnlyList<BudgetPlanDto>>
	{
		private readonly IBudgetPlanReadRepository _budgetPlanRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IFollowerReadRepository _followerRepo;
		private readonly IMapper _mapper;
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		public GetFollowingBudgetPlansByPeriodQueryHandler(
			IMapper mapper, IBudgetPlanReadRepository budgetPlanRepo, ICurrentUserService currentUser, IFollowerReadRepository followerRepo,
			IBudgetPeriodReadRepository budgetPeriodRepo
		)
		{
			_budgetPlanRepo = budgetPlanRepo;
			_currentUser = currentUser;
			_followerRepo = followerRepo;	
			_mapper = mapper;
			_budgetPeriodRepo = budgetPeriodRepo;
		}

		public async Task<IReadOnlyList<BudgetPlanDto>> Handle(GetFollowingBudgetPlansByPeriodQuery query, CancellationToken cancellationToken)
		{
			Guard.AgainstNullOrEmptyGuid(query.PeriodId, nameof(query.PeriodId));
			var budgetPeriod = await _budgetPeriodRepo.ExistAsync(q => q.Id == query.PeriodId, cancellationToken);
			if (!budgetPeriod) throw new NotFoundException("Kỳ ngân sách không hợp lệ");

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
					&& p.BudgetPeriodId == query.PeriodId
				).ProjectTo<BudgetPlanDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}
}

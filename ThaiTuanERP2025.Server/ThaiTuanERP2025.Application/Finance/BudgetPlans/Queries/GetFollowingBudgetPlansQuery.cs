using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Repositories;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Services;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Queries
{
	public sealed record GetFollowingBudgetPlansQuery(Guid PeriodId) : IRequest<IReadOnlyList<BudgetPlanDto>>;

	public sealed class GetFollowingBudgetPlansQueryHandler : IRequestHandler<GetFollowingBudgetPlansQuery, IReadOnlyList<BudgetPlanDto>> {
		private readonly IBudgetPlanReadRepository _budgetPlanRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		private readonly IFollowerReadRepository _followerRepo;
		private readonly IMapper _mapper;
		private readonly IBudgetPlanPermissionService _budgetPlanPermission;
		public GetFollowingBudgetPlansQueryHandler(
			IBudgetPlanReadRepository budgetPlanRepo, ICurrentUserService currentUser, IBudgetPeriodReadRepository budgetPeriodRepo,
			IFollowerReadRepository followerRepo, IMapper mapper, IBudgetPlanPermissionService budgetPlanPermission
		) {
			_budgetPlanRepo = budgetPlanRepo;
			_currentUser = currentUser;
			_budgetPeriodRepo = budgetPeriodRepo;
			_followerRepo = followerRepo;
			_mapper = mapper;
			_budgetPlanPermission = budgetPlanPermission;	
		}

		public async Task<IReadOnlyList<BudgetPlanDto>> Handle(GetFollowingBudgetPlansQuery query, CancellationToken cancellationToken) {
			var budgetPeriod = await _budgetPeriodRepo.ExistAsync(q => q.Id == query.PeriodId, cancellationToken);
			if (!budgetPeriod) throw new NotFoundException("Không tìm thấy kế hoạch ngân sách");

			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");

			var following = await _followerRepo.GetAllAsync(
				q => q.UserId == userId  && q.DocumentType == DocumentType.BudgetPlan,
				cancellationToken: cancellationToken
			);
			if(!following.Any()) return Array.Empty<BudgetPlanDto>();

			var documentIds = following.Select(f => f.DocumentId).ToList();
			if (!documentIds.Any())
				return Array.Empty<BudgetPlanDto>();

			var planDtos = await _budgetPlanRepo.ListProjectedAsync(
				q => q.Where(p => documentIds.Contains(p.Id)
					&& p.IsActive
					&& p.BudgetPeriodId == query.PeriodId
				).ProjectTo<BudgetPlanDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
			if (!planDtos.Any()) return Array.Empty<BudgetPlanDto>();

			_budgetPlanPermission.ApplyPermissions(planDtos, userId);

			return planDtos;
		}

		public sealed class GetFollowingBudgetPlansQueryValidator : AbstractValidator<GetFollowingBudgetPlansQuery>
		{
			public GetFollowingBudgetPlansQueryValidator() {
				RuleFor(x => x.PeriodId)
					.NotEmpty().WithMessage("Kỳ ngân sách không được để trống");
			}
		}
	}
}

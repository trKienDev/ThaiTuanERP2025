using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Queries
{
	public sealed record GetBudgetPlanByIdQuery(Guid Id) : IRequest<BudgetPlanDto>;

	public sealed class GetBudgetPlanByIdQueryHandler : IRequestHandler<GetBudgetPlanByIdQuery, BudgetPlanDto> {
		private readonly IBudgetPlanReadRepository _budgetPlanRepo;
		private readonly ICurrentUserService _currentUser;
		public GetBudgetPlanByIdQueryHandler(IBudgetPlanReadRepository budgetPlanRepo, ICurrentUserService currentUser)
		{
			_budgetPlanRepo = budgetPlanRepo;
			_currentUser = currentUser;
		}

		public async Task<BudgetPlanDto> Handle(GetBudgetPlanByIdQuery query, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");
			
			var budgetPlan = await _budgetPlanRepo.GetByIdProjectedAsync(query.Id, cancellationToken)
				?? throw new NotFoundException("Kế hoạch ngân sách không tồn tại");

			budgetPlan.CanReview = budgetPlan.SelectedReviewerId == userId;

			return budgetPlan;
		}

		public sealed class GetBudgetPlanByIdQueryValidator : AbstractValidator<GetBudgetPlanByIdQuery>
		{
			public GetBudgetPlanByIdQueryValidator()
			{
				RuleFor(x => x.Id)
					.NotEmpty().WithMessage("Id của Kế hoạch ngân sách không được để trống");
			}
		}
	}
}

using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Repositories;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Services;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Queries
{
	public sealed record GetBudgetPlanByIdQuery(Guid Id) : IRequest<BudgetPlanDto>;

	public sealed class GetBudgetPlanByIdQueryHandler : IRequestHandler<GetBudgetPlanByIdQuery, BudgetPlanDto> {
		private readonly IBudgetPlanReadRepository _budgetPlanRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IBudgetPlanPermissionService _budgetPlanPermission;
		public GetBudgetPlanByIdQueryHandler(IBudgetPlanReadRepository budgetPlanRepo, ICurrentUserService currentUser, IBudgetPlanPermissionService budgetPlanPermission)
		{
			_budgetPlanRepo = budgetPlanRepo;
			_currentUser = currentUser;
			_budgetPlanPermission = budgetPlanPermission;
		}

		public async Task<BudgetPlanDto> Handle(GetBudgetPlanByIdQuery query, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");
			
			var planDto = await _budgetPlanRepo.GetByIdProjectedAsync(query.Id, cancellationToken)
				?? throw new NotFoundException("Kế hoạch ngân sách không tồn tại");

			_budgetPlanPermission.ApplyPermissions(planDto, userId);

			return planDto;
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

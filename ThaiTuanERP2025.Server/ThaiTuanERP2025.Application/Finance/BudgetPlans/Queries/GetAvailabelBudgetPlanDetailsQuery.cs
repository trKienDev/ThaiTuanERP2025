using MediatR;
using ThaiTuanERP2025.Application.Account.Departments;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Queries
{
	public sealed record GetAvailabelBudgetPlanDetailsQuery() : IRequest<IReadOnlyList<BudgetPlanDetailDto>>;

	public sealed class GetAvailabelBudgetPlanDetailsQueryHandler : IRequestHandler<GetAvailabelBudgetPlanDetailsQuery, IReadOnlyList<BudgetPlanDetailDto>>
	{
		private readonly IBudgetPeriodReadRepository _budgetPeriodRepo;
		private readonly IBudgetPlanReadRepository _budgetPlanRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IUserReadRepostiory _userRepo;
		private readonly IDepartmentReadRepository _departmentRepo;
		public GetAvailabelBudgetPlanDetailsQueryHandler(
			IBudgetPlanReadRepository budgetPlanRepo, IBudgetPeriodReadRepository budgetPeriodRepo, ICurrentUserService currentUser,
			IUserReadRepostiory userRepo, IDepartmentReadRepository departmentRepo
		)
		{
			_budgetPeriodRepo = budgetPeriodRepo;
			_budgetPlanRepo = budgetPlanRepo;
			_currentUser = currentUser;
			_userRepo = userRepo;
			_departmentRepo = departmentRepo;
		}

		public async Task<IReadOnlyList<BudgetPlanDetailDto>> Handle(GetAvailabelBudgetPlanDetailsQuery query, CancellationToken cancellationToken)
		{
			var now = DateTime.UtcNow;
			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");
			var currentUser = await _userRepo.GetByIdProjectedAsync(userId, cancellationToken: cancellationToken)
				?? throw new NotFoundException("User không tồn tại");

			var userDepartment = await _departmentRepo.ExistAsync(q => q.Id == currentUser.DepartmentId, cancellationToken);
			if (!userDepartment) throw new BusinessRuleViolationException("User của bạn chưa có phòng ban, không thể làm thanh toán");

			var availablePeriodIds = await _budgetPeriodRepo.GetAvailablePeriodIdsAsync(cancellationToken);

			// available budget plans: current user department & available budget periods
			var availablePlans = await _budgetPlanRepo.GetAllAsync(
				q => q.DepartmentId == currentUser.DepartmentId 
					&& availablePeriodIds.Contains(q.BudgetPeriodId),
				cancellationToken: cancellationToken
			);

			return availablePlans.SelectMany(x => x.Details).ToList();
		}
	}
}

using MediatR;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.BudgetApprovers.Queries
{
	public sealed record GetBudgetApproversByUserDepartmentQuery() : IRequest<IReadOnlyList<BudgetApproverDto>>;

	public sealed class GetBudgetApproversByUserDepartmentQueryHandler : IRequestHandler<GetBudgetApproversByUserDepartmentQuery, IReadOnlyList<BudgetApproverDto>> {
		private readonly IBudgetApproverReadRepository _budgetApproverRepo;
		private readonly ICurrentUserService _currentUser;
		private readonly IUserReadRepostiory _userRepo;
		public GetBudgetApproversByUserDepartmentQueryHandler (
			IBudgetApproverReadRepository budgetApprovalRepo, ICurrentUserService currentUser, IUserReadRepostiory userRepo
		) {
			_budgetApproverRepo = budgetApprovalRepo;
			_currentUser = currentUser;
			_userRepo = userRepo;
		}

		public async Task<IReadOnlyList<BudgetApproverDto>> Handle(GetBudgetApproversByUserDepartmentQuery query, CancellationToken cancellationToken) {
			var userId = _currentUser.UserId ?? throw new InvalidOperationException("Không tìm thấy thông tin người dùng hiện tại");

			var userDto = await _userRepo.GetByIdProjectedAsync(userId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy user yêu cầu");

			return await _budgetApproverRepo.ListAsync(
				q => q.Where(x => x.Departments.Any(d => d.DepartmentId == userDto.DepartmentId))
			);
		}
	}
}

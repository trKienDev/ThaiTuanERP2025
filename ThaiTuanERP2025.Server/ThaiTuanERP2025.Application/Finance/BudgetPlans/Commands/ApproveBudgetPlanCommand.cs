using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands
{
	public sealed record ApproveBudgetPlanCommand(Guid planId) :  IRequest<Unit>;
	public sealed class ApproveBudgetPlanCommandHandler : IRequestHandler<ApproveBudgetPlanCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		public ApproveBudgetPlanCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
		{
			_uow = uow;
			_currentUser = currentUser;
		}

		public async Task<Unit> Handle(ApproveBudgetPlanCommand command, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");

			var budgetPlan = await _uow.BudgetPlans.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == command.planId && x.IsActive && !x.IsDeleted),
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Kế hoạch ngân sách không tồn tại");

			if (budgetPlan.Status != Domain.Finance.Enums.BudgetPlanStatus.Reviewed)
				throw new BusinessRuleViolationException("Kế hoạch ngân sách chưa được xem xét");

			if (budgetPlan.SelectedApproverId != userId)
				throw new ForbiddenException("Bạn không có quyền phê duyệt kế hoạch ngân sách này");

			var approver = await _uow.BudgetApprovers.GetByIdAsync(budgetPlan.SelectedApproverId, cancellationToken)
				?? throw new NotFoundException("User phê duyệt không tồn tại");

			budgetPlan.Approve(budgetPlan.SelectedApproverId, approver.SlaHours);

			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		public sealed class ApproveBudgetPlanCommandValidator : AbstractValidator<ApproveBudgetPlanCommand>
		{
			public ApproveBudgetPlanCommandValidator()
			{
				RuleFor(x => x.planId)
					.NotEmpty().WithMessage("Id tham chiếu không hợp lệ");
			}
		}
	}
}

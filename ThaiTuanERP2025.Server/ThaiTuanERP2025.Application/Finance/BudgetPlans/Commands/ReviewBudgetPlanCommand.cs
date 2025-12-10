using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands
{
	public sealed record ReviewBudgetPlanCommand(Guid Id) : IRequest<Unit>;

	public sealed class ReviewBudgetPlanCommandHandler : IRequestHandler<ReviewBudgetPlanCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		public ReviewBudgetPlanCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
		{
			_uow = uow;
			_currentUser = currentUser;
		}

		public async Task<Unit> Handle(ReviewBudgetPlanCommand command, CancellationToken cancellationToken)
		{
			var userId = _currentUser.UserId ?? throw new NotFoundException("User của bạn không hợp lệ");

			var plan = await _uow.BudgetPlans.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == command.Id && x.IsActive && !x.IsDeleted),
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy kế hoạch ngân sách");

			if (plan.Status != BudgetPlanStatus.Draft)
				throw new BusinessRuleViolationException($"Kế hoạch ngân sách đang ở trạng thái {plan.Status}, không thể xem xét.");
			if (plan.SelectedReviewerId != userId)
				throw new ForbiddenException("Bạn không phải user xem xét được chỉ định");

			plan.MarkReviewed(userId);

			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		public sealed class ReviewBudgetPlanCommandValidator : AbstractValidator<ReviewBudgetPlanCommand>
		{
			public ReviewBudgetPlanCommandValidator()
			{
				RuleFor(x => x.Id)
					.NotEmpty().WithMessage("Không có Id tham chiếu");
			}
		}
	}
}


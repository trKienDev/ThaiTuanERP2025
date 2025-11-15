using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands
{
	public sealed record ReviewBudgetPlanCommand(Guid BudgetPlanId) : IRequest<Unit>;

	public sealed class ReviewBudgetPlanCommandHanlder : IRequestHandler<ReviewBudgetPlanCommand, Unit> {
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		public ReviewBudgetPlanCommandHanlder(IUnitOfWork uow, ICurrentUserService currentUser) {
			_uow = uow;
			_currentUser = currentUser;
		}

		public async Task<Unit> Handle(ReviewBudgetPlanCommand command, CancellationToken cancellationToken) {
			Guard.AgainstNullOrEmptyGuid(command.BudgetPlanId, nameof(command.BudgetPlanId));

			var userId = _currentUser.UserId ?? throw new NotFoundException("user không hợp lệ");

			var plan = await _uow.BudgetPlans.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == command.BudgetPlanId),
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Kế hoạch ngân sách không tồn tại");

			plan.MarkReviewed(userId);

			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

	}
}

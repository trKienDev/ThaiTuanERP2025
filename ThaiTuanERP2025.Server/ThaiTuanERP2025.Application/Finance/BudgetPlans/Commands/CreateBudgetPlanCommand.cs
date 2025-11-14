using MediatR;
using System.Net.WebSockets;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands
{
	public sealed record CreateBudgetPlanCommand(
		Guid DepartmentId,
		Guid BudgetCodeId,
		Guid BudgetPeriodId,
		decimal Amount,
		Guid ReviewerId, 
		Guid ApproverId
	) : IRequest<Unit>;

	public sealed class CreateBudgetPlanCommandHandler : IRequestHandler<CreateBudgetPlanCommand, Unit> {
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		public CreateBudgetPlanCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
		{
			_uow = uow;
			_currentUser = currentUser;
		}

		public async Task<Unit> Handle(CreateBudgetPlanCommand command, CancellationToken cancellationToken) {
			Guard.AgainstNullOrEmptyGuid(command.DepartmentId, nameof(command.DepartmentId));
			Guard.AgainstNullOrEmptyGuid(command.BudgetPeriodId, nameof(command.BudgetPeriodId));
			Guard.AgainstNullOrEmptyGuid(command.BudgetCodeId, nameof(command.BudgetCodeId));
			Guard.AgainstNullOrEmptyGuid(command.ReviewerId, nameof(command.ReviewerId));
			Guard.AgainstNullOrEmptyGuid(command.ApproverId, nameof(command.ApproverId));
			Guard.AgainstNegativeOrZero(command.Amount, nameof(command.Amount));

			var department = await _uow.Departments.ExistAsync(q => q.Id ==  command.DepartmentId, cancellationToken);
			if (!department) throw new NotFoundException("Không tìm thấy phòng ban");

			var budgetPeriod = await _uow.BudgetPeriods.ExistAsync(q => q.Id == command.BudgetPeriodId, cancellationToken);
			if (!budgetPeriod) throw new NotFoundException("Không tìm thấy kỳ ngân sách");

			var budgetCode = await _uow.BudgetCodes.ExistAsync(q => q.Id == command.BudgetCodeId, cancellationToken);
			if (!budgetCode) throw new NotFoundException("Không tìm thấy mã ngân sách");

			var reviewer = await _uow.Users.ExistAsync(q => q.Id == command.ReviewerId, cancellationToken);
			if (!reviewer) throw new NotFoundException("Không tìm thấy user xem xét");

			var approver = await _uow.Users.ExistAsync(q => q.Id == command.ApproverId, cancellationToken);
			if (!approver) throw new NotFoundException("Không tìm thấy user phê duyệt");

			// check Exist
			var planExist = await _uow.BudgetPlans.ExistAsync(
				q => q.BudgetCodeId == command.BudgetCodeId
					&& q.BudgetPeriodId == command.BudgetPeriodId
					&& q.DepartmentId == command.DepartmentId,
				cancellationToken: cancellationToken
			);
			if (planExist) throw new ConflictException("Kế hoạch ngân sách này đã tồn tại");

			var entity = new BudgetPlan(command.DepartmentId, command.BudgetCodeId, command.BudgetPeriodId, command.Amount, command.ReviewerId, command.ApproverId);
			await _uow.BudgetPlans.AddAsync(entity, cancellationToken);

			var creatorId = _currentUser.UserId ?? throw new InvalidOperationException("CreatedByUserId is null, không thể auto-follow BudgetPlan.");
			var reviewerId = entity.SelectedReviewerId;
			var approverId = entity.SelectedApproverId;
			var followers = new List<Follower>
			{
				new Follower(entity.Id, SubjectType.BudgetPlan, creatorId),
				new Follower(entity.Id, SubjectType.BudgetPlan, reviewerId),
				new Follower(entity.Id, SubjectType.BudgetPlan, approverId)
			};
			await _uow.Followers.AddRangeAsync(followers, cancellationToken);

			await _uow.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}

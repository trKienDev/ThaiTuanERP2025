using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands
{
	public sealed record CreateBudgetPlanCommand(BudgetPlanRequest Request) : IRequest<Unit>;

	public sealed class CreateBudgetPlanCommandHandler : IRequestHandler<CreateBudgetPlanCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		public CreateBudgetPlanCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
		{
			_uow = uow;
			_currentUser = currentUser;
		}

		public async Task<Unit> Handle(CreateBudgetPlanCommand command, CancellationToken cancellationToken) {
			var request = command.Request;

			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");

			var duplicatedCodes = request.Details.GroupBy(d => d.BudgetCodeId)
				.Where(g => g.Count() > 1)
				.ToList();

			if (duplicatedCodes.Any())
				throw new DomainException("Một mã ngân sách không được nhập nhiều lần.");

			var department = await _uow.Departments.ExistAsync(q => q.Id == request.DepartmentId, cancellationToken);
			if (!department) throw new NotFoundException("Không tìm thấy phòng ban yêu cầu");

			var budgetPeriod = await _uow.BudgetPeriods.ExistAsync(q => q.Id == request.BudgetPeriodId, cancellationToken);
			if (!budgetPeriod) throw new NotFoundException("Không tim thấy kỳ ngân sách");

			var selectedReviewer = await _uow.Users.ExistAsync(q => q.Id == request.SelectedReviewerId, cancellationToken);
			if (!selectedReviewer) throw new NotFoundException("User xem xét không hợp lệ");

			var selectedApprover = await _uow.Users.ExistAsync(q => q.Id == request.SelectedApproverId, cancellationToken);
			if (!selectedApprover) throw new NotFoundException("User phê duyệt không hợp lệ");

			// ==== Budget Plan ====
			var budgetPlan = new BudgetPlan(
				request.DepartmentId,
				request.BudgetPeriodId,
				request.SelectedReviewerId,
				request.SelectedApproverId
			);

			foreach (var d in request.Details)
			{
				var code = await _uow.BudgetCodes.ExistAsync(q => q.Id == d.BudgetCodeId, cancellationToken);
				if (!code) throw new DirectoryNotFoundException("Mã ngân sách không tồn tại");

				budgetPlan.AddDetail(d.BudgetCodeId, d.Amount);
			}

			await _uow.BudgetPlans.AddAsync(budgetPlan, cancellationToken);

			// ==== Follower ====
			var followers = new List<Follower> {
				new Follower(budgetPlan.Id, DocumentType.BudgetPlan, userId),
				new Follower(budgetPlan.Id, DocumentType.BudgetPlan, request.SelectedApproverId),
				new Follower(budgetPlan.Id, DocumentType.BudgetPlan, request.SelectedReviewerId)
			};
			await _uow.Followers.AddRangeAsync(followers, cancellationToken);

			await _uow.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		public sealed class CreateBudgetPlanCommandValidator : AbstractValidator<CreateBudgetPlanCommand>
		{
			public CreateBudgetPlanCommandValidator()
			{
				RuleFor(x => x.Request.DepartmentId)
					.NotEmpty().WithMessage("Phòng ban không được để trống.");

				RuleFor(x => x.Request.BudgetPeriodId)
					.NotEmpty().WithMessage("Kỳ ngân sách không được để trống.");

				RuleFor(x => x.Request.SelectedReviewerId)
					.NotEmpty().WithMessage("Người xem xét không được để trống.");

				RuleFor(x => x.Request.SelectedApproverId)
					.NotEmpty().WithMessage("Người phê duyệt không được để trống.");

				RuleFor(x => x.Request.Details)
					.NotEmpty().WithMessage("Phải có ít nhất 1 chi tiết.")
					.Must(d => d.Any())
					.WithMessage("Danh sách chi tiết không được rỗng.");

				RuleForEach(x => x.Request.Details).ChildRules(detail =>
				{
					detail.RuleFor(d => d.BudgetCodeId)
						.NotEmpty().WithMessage("Mã ngân sách không được trống.");

					detail.RuleFor(d => d.Amount)
						.GreaterThan(0).WithMessage("Số tiền phải lớn hơn 0.");
				});
			}
		}
	}
}

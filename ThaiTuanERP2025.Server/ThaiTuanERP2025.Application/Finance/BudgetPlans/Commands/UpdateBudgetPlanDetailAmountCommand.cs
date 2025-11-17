using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands
{
	public sealed record UpdateBudgetPlanDetailAmountCommand(Guid DetailId, decimal Amount) : IRequest<Unit>;
	public sealed class UpdateBudgetPlanDetailAmountCommandHandler : IRequestHandler<UpdateBudgetPlanDetailAmountCommand, Unit> {
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		public UpdateBudgetPlanDetailAmountCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
		{
			_uow = uow;
			_currentUser = currentUser;	
		}

		public async Task<Unit> Handle(UpdateBudgetPlanDetailAmountCommand command, CancellationToken cancellationToken) {
			var userId = _currentUser.UserId ?? throw new NotFoundException("User không hợp lệ");
			
			var budgetPlanDetail = await _uow.BudgetPlanDetails.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == command.DetailId),
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy chi tiết kế hoạch ngân sách");

			budgetPlanDetail.UpdateAmount(command.Amount, userId);
			
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		public sealed class UpdateBudgetPlanDetailAmountCommandValidator : AbstractValidator<UpdateBudgetPlanDetailAmountCommand>
		{
			public UpdateBudgetPlanDetailAmountCommandValidator()
			{
				RuleFor(x => x.DetailId)
					.NotEmpty().WithMessage("Thiếu định danh chi tiết");

				RuleFor(x => x.Amount)
					.GreaterThan(1000).WithMessage("Số tiền phải lơn hơn 1000");
			}
		}
	}
}

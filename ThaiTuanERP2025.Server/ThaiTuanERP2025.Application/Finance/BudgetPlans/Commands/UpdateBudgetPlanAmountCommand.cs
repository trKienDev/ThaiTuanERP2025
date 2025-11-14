using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands
{
	public sealed record UpdateBudgetPlanAmountCommand(Guid BudgetPlanId, decimal Amount) : IRequest<Unit>;
	public sealed class UpdateBudgetPlanAmountCommandHandler : IRequestHandler<UpdateBudgetPlanAmountCommand, Unit> {
		private readonly IUnitOfWork _uow;
		public UpdateBudgetPlanAmountCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(UpdateBudgetPlanAmountCommand command, CancellationToken cancellationToken) {
			Guard.AgainstNullOrEmptyGuid(command.BudgetPlanId, nameof(command.BudgetPlanId));
			Guard.AgainstNegativeOrZero(command.Amount, nameof(command.Amount));

			var plan = await _uow.BudgetPlans.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == command.BudgetPlanId),
					asNoTracking: false,
					cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Kế hoạch ngân sách không tồn tại");

			plan.SetAmount(command.Amount);
			
			await _uow.SaveChangesAsync(cancellationToken);	
			return Unit.Value;
		}
	}
}

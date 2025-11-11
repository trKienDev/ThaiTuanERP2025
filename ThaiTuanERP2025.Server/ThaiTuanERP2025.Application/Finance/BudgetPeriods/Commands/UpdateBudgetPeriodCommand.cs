using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Commands
{
	public sealed record UpdateBudgetPeriodRequest(DateTime StartDate, DateTime EndDate);
	public sealed record UpdateBudgetPeriodCommand(Guid Id, UpdateBudgetPeriodRequest Request) : IRequest<Unit>;

	public sealed class UpdateBudgetPeriodCommandHandler : IRequestHandler<UpdateBudgetPeriodCommand, Unit> {
		private readonly IUnitOfWork _uow;
		public UpdateBudgetPeriodCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(UpdateBudgetPeriodCommand command, CancellationToken cancellationToken) { 
			var request = command.Request;

			var periodId = command.Id;
			Guard.AgainstNullOrEmptyGuid(periodId, nameof(periodId));

			var budgetPeriod = await _uow.BudgetPeriods.SingleOrDefaultAsync(
				q => q.Where(bp => bp.Id == periodId),
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new DirectoryNotFoundException("Không tìm thấy kỳ ngân sách được yêu cầu");

			budgetPeriod.SetStartDate(request.StartDate);
			budgetPeriod.SetEndDate(request.EndDate);

			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}

	public sealed class UpdateBudgetPeriodCommandValidator : AbstractValidator<UpdateBudgetPeriodCommand> {
		public UpdateBudgetPeriodCommandValidator() {
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id kỳ ngân sách không được để trống");

			RuleFor(x => x.Request.StartDate).NotEmpty().WithMessage("Ngày bắt đầu không được để trống");

			RuleFor(x => x.Request.EndDate).NotEmpty().WithMessage("Ngày kết thúc không được để trống");

			RuleFor(x => x).Must(x => x.Request.EndDate >= x.Request.StartDate).WithMessage("Ngày kết thúc không được nhỏ hơn ngày bắt đầu");
		}
	}
}

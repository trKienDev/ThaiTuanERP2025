using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Budgets.Commands.BudgetPeriods.CreateBudgetPeriod
{
	public class CreateBudgetPeriodHandler : IRequestHandler<CreateBudgetPeriodCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateBudgetPeriodHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(CreateBudgetPeriodCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;

			var exists = await _unitOfWork.BudgetPeriods.AnyAsync(
				x => x.Year == request.Year && x.Month == request.Month,
				cancellationToken
			);
			if (exists) throw new ConflictException("Kỳ ngân sách đã tồn tại");

			var entity = new BudgetPeriod(request.Year, request.Month, request.StartDate, request.EndDate);

			await _unitOfWork.BudgetPeriods.AddAsync(entity, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}

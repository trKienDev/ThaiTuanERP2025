using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Commands.CreateForYear
{
	public sealed class CreateBudgetPeriodForYearCommandHandler : IRequestHandler<CreateBudgetPeriodsForYearCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public CreateBudgetPeriodForYearCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(CreateBudgetPeriodsForYearCommand command, CancellationToken cancellationToken)
		{
			var year = command.Year;
			var existingPeriod = await _uow.BudgetPeriods.ExistAsync(p => p.Year == year, cancellationToken);
			if (existingPeriod)
				throw new ConflictException($"Kỳ ngân sách đã tồn tại cho năm {year}");

			var newPeriods = new List<BudgetPeriod>();

			for (int month = 1; month <= 12; month++)
			{
				DateTime startDate;
				DateTime endDate;

				if (month == 1)
				{
					// Kỳ tháng 1 → dùng tháng 12 năm trước
					startDate = new DateTime(year - 1, 12, 20);
					endDate = new DateTime(year - 1, 12, DateTime.DaysInMonth(year - 1, 12));
				}
				else
				{
					// Kỳ tháng N → dùng tháng (N−1) trong cùng năm
					startDate = new DateTime(year, month - 1, 20);
					endDate = new DateTime(year, month - 1, DateTime.DaysInMonth(year, month - 1));
				}

				var period = new BudgetPeriod(year, month, startDate, endDate);
				newPeriods.Add(period);
			}

			await _uow.BudgetPeriods.AddRangeAsync(newPeriods, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}

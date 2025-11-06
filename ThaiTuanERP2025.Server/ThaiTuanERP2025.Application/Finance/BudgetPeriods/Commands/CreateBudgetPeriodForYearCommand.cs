using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Commands
{
	public record CreateBudgetPeriodsForYearCommand(int Year) : IRequest<Unit>;
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
			if (await _uow.BudgetPeriods.ExistAsync(p => p.Year == year, cancellationToken))
				throw new ConflictException($"Kỳ ngân sách đã tồn tại cho năm {year}");

			var newPeriods = new List<BudgetPeriod>();

			for (int month = 1; month <= 12; month++)
			{
				int prevMonth = month == 1 ? 12 : month - 1;
				int prevYear = month == 1 ? year - 1 : year;

				var startDate = UtcDate(prevYear, prevMonth, 20);
				var endDate = UtcDate(prevYear, prevMonth, DateTime.DaysInMonth(prevYear, prevMonth));

				newPeriods.Add(new BudgetPeriod(year, month, startDate, endDate));
			}

			await _uow.BudgetPeriods.AddRangeAsync(newPeriods, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		private static DateTime UtcDate(int y, int m, int d) => new DateTime(y, m, d, 0, 0, 0, DateTimeKind.Utc);
	}
}

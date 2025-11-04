using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Commands.CreateForYear
{
	public record CreateBudgetPeriodsForYearCommand(int Year) : IRequest<Unit>;
}

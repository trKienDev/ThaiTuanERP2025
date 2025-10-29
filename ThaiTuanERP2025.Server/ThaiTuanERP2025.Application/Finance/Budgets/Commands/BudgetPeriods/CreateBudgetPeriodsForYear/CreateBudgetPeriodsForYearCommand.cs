using MediatR;

namespace ThaiTuanERP2025.Application.Finance.Budgets.Commands.BudgetPeriods.CreateBudgetPeriodsForYear
{
	public record CreateBudgetPeriodsForYearCommand(int Year) : IRequest<Unit>;
}

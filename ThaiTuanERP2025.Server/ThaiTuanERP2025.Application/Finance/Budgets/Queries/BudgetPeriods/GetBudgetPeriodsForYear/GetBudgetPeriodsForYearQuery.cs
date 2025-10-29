using MediatR;
using ThaiTuanERP2025.Application.Finance.Budgets.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Budgets.Queries.BudgetPeriods.GetBudgetPeriodsForYear
{
	public sealed record GetBudgetPeriodsForYearQuery(int Year) : IRequest<IReadOnlyList<BudgetPeriodDto>>;
}

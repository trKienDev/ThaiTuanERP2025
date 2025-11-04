using MediatR;
using ThaiTuanERP2025.Application.Finance.Budgets.DTOs;

namespace ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries.GetForYear
{
	public sealed record GetBudgetPeriodsForYearQuery(int Year) : IRequest<IReadOnlyList<BudgetPeriodDto>>;
}

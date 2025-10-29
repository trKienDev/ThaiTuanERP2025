using MediatR;
using ThaiTuanERP2025.Application.Finance.Budgets.Requests;

namespace ThaiTuanERP2025.Application.Finance.Budgets.Commands.BudgetPeriods.CreateBudgetPeriod
{
	public record CreateBudgetPeriodCommand(BudgetPeriodRequest Request) : IRequest<Unit>;
}

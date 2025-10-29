using MediatR;
using ThaiTuanERP2025.Application.Finance.Budgets.Requests;

namespace ThaiTuanERP2025.Application.Finance.Budgets.Commands.BudgetGroups.CreateBudgetGroup
{
	public sealed record CreateBudgetGroupCommand(BudgetGroupRequest Request) : IRequest<Unit>;
}

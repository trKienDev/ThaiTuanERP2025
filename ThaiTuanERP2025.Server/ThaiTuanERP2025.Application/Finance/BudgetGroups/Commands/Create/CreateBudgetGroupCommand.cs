using MediatR;

namespace ThaiTuanERP2025.Application.Finance.BudgetGroups.Commands.Create
{
	public sealed record CreateBudgetGroupCommand(string Name, string Code) : IRequest<Unit>;
}

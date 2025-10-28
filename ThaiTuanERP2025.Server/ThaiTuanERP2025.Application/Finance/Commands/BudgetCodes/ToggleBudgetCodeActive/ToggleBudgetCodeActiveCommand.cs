using MediatR;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.ToggleBudgetCodeActive
{
	public sealed record ToggleBudgetCodeActiveCommand(Guid Id) : IRequest<Unit>;
}

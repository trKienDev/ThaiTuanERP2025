using MediatR;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeridos.CreateBudgetPeriod
{
	public record CreateBudgetPeriodCommand( BudgetPeriodRequest Request ) : IRequest<BudgetPeriodDto>;
}

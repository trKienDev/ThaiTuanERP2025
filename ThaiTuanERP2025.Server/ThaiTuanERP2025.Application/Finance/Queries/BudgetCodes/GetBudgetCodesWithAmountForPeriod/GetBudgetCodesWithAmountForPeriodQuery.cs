using MediatR;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetBudgetCodesWithAmountForPeriod
{
	public record GetBudgetCodesWithAmountForPeriodQuery(int? Year, int? Month) : IRequest<List<BudgetCodeWithAmountDto>>;
}

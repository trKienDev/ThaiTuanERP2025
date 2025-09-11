using MediatR;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.CreateBudgetCode
{
	public class CreateBudgetCodeCommand : IRequest<BudgetCodeDto>
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public Guid BudgetGroupId { get; set; }
	}
}

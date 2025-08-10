using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Dtos;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.UpdateBudgetPlan
{
	public class UpdateBudgetPlanCommand : IRequest<BudgetPlanDto>
	{
		public Guid Id { get; init; }
		public decimal Amount { get; init; }
		public string Status { get; init; } = "Draft";
	}
}

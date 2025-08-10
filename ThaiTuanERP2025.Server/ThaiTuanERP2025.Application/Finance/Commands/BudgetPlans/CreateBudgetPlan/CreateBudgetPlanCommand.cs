using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Dtos;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.CreateBudgetPlan
{
	public class CreateBudgetPlanCommand : IRequest<BudgetPlanDto>
	{
		public Guid DepartmentId { get; set; }
		public Guid BudgetCodeId { get; set; }
		public Guid BudgetPeriodId { get; set; }
		public decimal Amount { get; set; }
	}
}

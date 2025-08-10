using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Dtos;

namespace ThaiTuanERP2025.Application.Finance.Queries.BudgetPlans.GetAllBudgetPlans
{
	public record GetAllBudgetPlansQuery : IRequest<List<BudgetPlanDto>> { }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeridos.CreateBudgetPeriod
{
	public record CreateBudgetPeriodCommand(
		int Year,
		int Month
	) : IRequest<BudgetPeriodDto>;
}

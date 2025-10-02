using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeriods.DeleteBudgetPeriod
{
	public record DeleteBudgetPeriodCommand(Guid Id): IRequest;
}

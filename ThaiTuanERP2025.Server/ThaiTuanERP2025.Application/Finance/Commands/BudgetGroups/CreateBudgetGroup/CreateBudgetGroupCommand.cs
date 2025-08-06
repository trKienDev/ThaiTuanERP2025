using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.CreateBudgetGroup
{
	public class CreateBudgetGroupCommand : IRequest<BudgetGroupDto>
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
	}
}

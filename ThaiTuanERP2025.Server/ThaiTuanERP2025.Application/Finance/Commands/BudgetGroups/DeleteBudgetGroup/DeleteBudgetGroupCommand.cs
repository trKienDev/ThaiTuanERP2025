using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.DeleteBudgetGroup
{
	public class DeleteBudgetGroupCommand : IRequest
	{
		public Guid Id { get; set; }
		public DeleteBudgetGroupCommand(Guid id)
		{
			Id = id;
		}
	}
}

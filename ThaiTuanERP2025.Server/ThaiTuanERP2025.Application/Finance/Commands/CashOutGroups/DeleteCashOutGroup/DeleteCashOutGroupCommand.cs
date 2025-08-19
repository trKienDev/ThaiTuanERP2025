using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.DeleteCashOutGroup
{
	public record DeleteCashOutGroupCommand(Guid Id): IRequest<bool>;
}

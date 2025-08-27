using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.DeleteCashoutCode
{
	public record DeleteCashoutCodeCommand(Guid Id) : IRequest<bool>;
}

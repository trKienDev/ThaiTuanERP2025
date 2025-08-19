using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.Taxes.DeleteTax
{
	public record DeleteTaxCommand(Guid Id) : IRequest<bool>;
}

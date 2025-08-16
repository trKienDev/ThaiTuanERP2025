using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.Suppliers.DeleteSupplier
{
	public record DeleteSupplierCommand(Guid Id) : IRequest<bool>;
}

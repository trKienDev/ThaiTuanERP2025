using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.Suppliers.ToggleSupplierStatus
{
	public record ToggleSupplierStatusCommand(Guid Id, bool IsActive) : IRequest<SupplierDto>;
}

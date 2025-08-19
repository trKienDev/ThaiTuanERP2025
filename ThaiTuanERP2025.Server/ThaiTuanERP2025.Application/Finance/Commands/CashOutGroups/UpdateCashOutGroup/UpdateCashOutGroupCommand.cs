using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.UpdateCashOutGroup
{
	public record UpdateCashOutGroupCommand(Guid Id, string Code, string Name, string? Description) : IRequest<CashOutGroupDto>;
}

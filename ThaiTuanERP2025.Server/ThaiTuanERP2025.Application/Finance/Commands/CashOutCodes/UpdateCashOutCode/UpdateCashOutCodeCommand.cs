using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.UpdateCashoutCode
{
	public record UpdateCashoutCodeCommand(
		Guid Id, string Code, string Name, string? Description,
		Guid CashOutGroupId, Guid PostingLedgerAccountId
	) : IRequest<CashoutCodeDto>;
}

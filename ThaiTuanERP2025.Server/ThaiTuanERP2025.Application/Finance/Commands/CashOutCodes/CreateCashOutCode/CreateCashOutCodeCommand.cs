using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutCodes.CreateCashOutCode
{
	public record CreateCashOutCodeCommand(
		string Code, string Name, Guid CashOutGroupId, Guid PostingLedgerAccountId, string? Description
	) : IRequest<CashOutCodeDto>;
}

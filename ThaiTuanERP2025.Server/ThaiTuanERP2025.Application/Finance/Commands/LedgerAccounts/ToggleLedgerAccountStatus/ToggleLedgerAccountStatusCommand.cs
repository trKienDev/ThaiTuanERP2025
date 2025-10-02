using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.ToggleLedgerAccountStatus
{
	public record ToggleLedgerAccountStatusCommand(Guid Id, bool IsActive) : IRequest<bool>;
}

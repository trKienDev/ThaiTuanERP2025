using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.UpdateLedgerAccount
{
	public record UpdateLedgerAccountCommand(Guid Id, string Number, string Name, Guid LedgerAccountTypeId, string? Description) : IRequest<LedgerAccountDto>;
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.DTOs;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.CreateLedgerAccount
{
	public record CreateLedgerAccountCommand(
		string Number, string Name, Guid LedgerAccountTypeId, Guid? ParentLedgerAccountId,
		string? Description
	) : IRequest<LedgerAccountDto>;
}

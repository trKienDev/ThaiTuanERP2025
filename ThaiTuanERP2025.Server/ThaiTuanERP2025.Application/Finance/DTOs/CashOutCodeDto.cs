using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record CashOutCodeDto(
		Guid Id, string Code, string Name, Guid CashOutGroupId, string CashOutGroupCode, string CashOutGroupName,
		Guid PostingLedgerAccountId, string PostingLedgerAccountNumber, string PostingLedgerAccountName,
		string? Description, bool IsActive
	);
}

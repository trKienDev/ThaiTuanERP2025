using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record LedgerAccountDto(
		Guid Id, 
		string Number, 
		string Name, 
		Guid LedgerAccountTypeId, 
		string LedgerAccounTypeName,
		Guid? ParentLedgerAccountId, 
		LedgerAccountBalanceType LedgerAccountBalanceType,
		string Path, 
		int Level,
		string? Description, 
		bool IsActive
	);
}

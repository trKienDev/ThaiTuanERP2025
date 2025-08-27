using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record LedgerAccountTypeDto(
		Guid Id, 
		string Code, 
		string Name, 
		LedgerAccountTypeKind LedgerAccountTypeKind, 
		string? Description, 
		bool IsActive);
}

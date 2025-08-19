using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record LedgerAccountDto(
		Guid Id, string Number, string Name, Guid AccountTypeId, Guid AccounTypeName,
		Guid ParrentAccountId, string Path, int Level ,string? Description, bool IsActive
	);
}

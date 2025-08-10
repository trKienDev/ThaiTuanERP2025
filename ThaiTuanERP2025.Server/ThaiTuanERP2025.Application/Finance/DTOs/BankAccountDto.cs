using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record BankAccountDto (
		Guid Id,
		string AccountNumber,
		string BankName,
		string? AccountHolder,
		Guid? DepartmentId,
		string? DepartmentName,
		string? CustomerName,
		bool IsActive,
		DateTime CreatedDate
	);
}

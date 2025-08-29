using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record SupplierDto(Guid Id, string Name, string? TaxCode, bool IsActive);
	public sealed record CreateSupplierRequest(string Name, string? TaxCode);
	public sealed record UpdateSupplierRequest(string Name, string? TaxCode, bool IsActive);

}

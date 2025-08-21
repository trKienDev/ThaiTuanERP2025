using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public class LedgerAccountTreeDto
	{
		public Guid Id { get; set; }
		public string Code { get; set; } = default!;
		public string Name { get; set; } = default!;
		public string? Description { get; set; }
		public bool IsActive { get; set; }
		public string BalanceType { get; set; } = default!;

		public List<LedgerAccountTreeDto> Children { get; set; } = new();
	}
}

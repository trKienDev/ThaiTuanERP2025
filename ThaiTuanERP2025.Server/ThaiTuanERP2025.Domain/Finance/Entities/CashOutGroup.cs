using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class CashOutGroup : AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public bool IsActive { get; set; } = true;

		public ICollection<CashOutCode> CashOutCodes { get; set; } = new List<CashOutCode>();
	}
}

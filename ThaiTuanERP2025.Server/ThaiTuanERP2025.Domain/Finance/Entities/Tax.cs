using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class Tax : AuditableEntity
	{
		public string PolicyName { get; set; } = null!;
		public decimal Rate { get; set; }

		public Guid PostingLedgerAccountId { get; set; }
		public string? Description { get; set; }	
		public bool IsActive { get; set; } = true;

		public LedgerAccount PostingLedgerAccount { get; set; } = null!;
	}
}

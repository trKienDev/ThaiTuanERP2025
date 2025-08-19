using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class CashOutCode : AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;	

		public Guid CashOutGroupId { get; set; }	
		public Guid PostingLedegerAccoutnId { get; set; }

		public string? Description { get; set; }
		public bool IsActive { get; set; } = true;

		public CashOutGroup CashOutGroup { get; set; } = null!;
		public LedgerAccount PostingLedgerAccount { get; set; } = null!;
	}
}

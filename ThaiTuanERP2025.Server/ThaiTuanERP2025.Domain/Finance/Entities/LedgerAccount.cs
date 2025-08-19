using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class LedgerAccount : AuditableEntity
	{
		public string Number { get; set; } = null!;
		public string Name { get; set; } = null!;
		public Guid? LedgerAccountTypeId { get; set; }	
		public Guid? ParrentLedgerAccountId { get; set; }

		// tối ưu cây
		public string Path { get; set; } = "/";
		public int Level { get; set; }

		public string? Description { get; set; }
		public bool IsActive { get; set; } = true;

		public LedgerAccountType LedgerAccountType { get; set; } = null!;
		public LedgerAccount? Parent { get; set; }
		public ICollection<LedgerAccount> Children { get; set; } = new List<LedgerAccount>();
	}
}

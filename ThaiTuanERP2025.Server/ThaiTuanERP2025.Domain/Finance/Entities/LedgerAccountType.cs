using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class LedgerAccountType : AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public LedgerAccountTypeKind LedgerAccountTypeKind { get; set; }
		public string? Description { get; set; }
		public bool IsActive { get; set; } = true;

		public ICollection<LedgerAccount> LedgerAccounts { get; set; } = new List<LedgerAccount>();
	}
}

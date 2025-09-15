using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class LedgerAccount : AuditableEntity
	{
		public string Number { get; set; } = null!;
		public string Name { get; set; } = null!;
		public Guid? LedgerAccountTypeId { get; set; }	
		public Guid? ParentLedgerAccountId { get; set; }

		// tối ưu cây
		public string Path { get; set; } = "/";
		public int Level { get; set; }

		public string? Description { get; set; }
		public bool IsActive { get; set; } = true;

		public LedgerAccountType LedgerAccountType { get; set; } = null!;
		public LedgerAccount? Parent { get; set; }
		public LedgerAccountBalanceType LedgerAccountBalanceType { get; set; }
		public ICollection<LedgerAccount> Children { get; set; } = new List<LedgerAccount>();

		public ICollection<CashoutCode> CashoutCodes { get; set; } = new List<CashoutCode>();
	}
}

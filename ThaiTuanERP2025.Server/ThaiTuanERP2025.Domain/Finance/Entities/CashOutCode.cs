using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class CashoutCode : AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;	

		public Guid CashoutGroupId { get; set; }	
		public Guid PostingLedgerAccountId { get; set; }

		public string? Description { get; set; }
		public bool IsActive { get; set; } = true;

		public CashoutGroup CashoutGroup { get; set; } = null!;
		public LedgerAccount PostingLedgerAccount { get; set; } = null!;
	}
}

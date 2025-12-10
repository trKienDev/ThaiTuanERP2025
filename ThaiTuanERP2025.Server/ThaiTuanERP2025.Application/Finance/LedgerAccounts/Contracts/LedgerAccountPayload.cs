using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts
{
	public sealed record LedgerAccountPayload
	{
		public string Number { get; set; } = null!;
		public string Name { get; set; } = null!;
		public LedgerAccountBalanceType BalanceType { get; set; }
		public string? Description { get; set; } = null;
                public Guid? LedgerAccountTypeId { get; set; } = null;
                public Guid? ParentLedgerAccountId { get; set; } = null;
	}
}

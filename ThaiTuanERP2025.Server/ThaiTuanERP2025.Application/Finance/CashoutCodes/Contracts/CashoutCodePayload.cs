namespace ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts
{
	public sealed record CashoutCodePayload
	{
		public string Name { get; set; } = default!;
		public Guid GroupId { get; set; }
		public Guid LedgerAccountId { get; set; }
		public string? Description { get; set; }
	}
}

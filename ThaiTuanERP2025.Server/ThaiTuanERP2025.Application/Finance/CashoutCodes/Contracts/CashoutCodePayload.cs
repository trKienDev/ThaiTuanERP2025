namespace ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts
{
	public sealed record CashoutCodePayload
	{
		public string Code { get; set; }
		public string Name { get; set; }	
		public Guid CashoutGroupId { get; set; }
		public Guid PostingLedgerAccountId { get; set; }
		public string? Description { get; set; }
	}
}

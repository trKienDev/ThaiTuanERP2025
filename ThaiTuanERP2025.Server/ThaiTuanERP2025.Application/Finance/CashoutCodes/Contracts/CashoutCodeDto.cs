namespace ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts
{
	public sealed record CashoutCodeDto
	{
		public Guid Id { get; init; } 
		public string Name { get; init; } = default!;
		public bool IsActive { get; init; }
		public Guid CashoutGroupId { get; init; }
		public string CashoutGroupName { get; init; } = default!;
		public Guid PostingLedgerAccountId { get; init; }
		public string PostingLedgerAccountName { get; init; } = default!;
                public string? Description { get; init; }
	};

	public sealed record CashoutCodeTreeDto
	{
                public Guid Id { get; init; } 
		public string Name { get; init; } = default!;
                public Guid CashoutGroupId { get; init; }
                public Guid PostingLedgerAccountId { get; init; }
		public string PostingLedgerAccountName { get; init; } = default!;
		public string? Description { get; init; }
        }
}

namespace ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts
{
	public sealed record CashoutGroupDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = null!;
		public Guid? ParentId { get; init; } = Guid.Empty;
		public CashoutGroupDto? ParentGroup { get; init; }
		public string? Description { get; init; } = null;
	}

	public sealed record CashoutGroupTreeDto
	{
		public Guid Id { get; init; }
		public Guid? ParentId { get; init; }
		public string Name { get; init; } = default!;
		public int Level { get; init; }
		public string Path { get; init; } = null!;
		public string? Description { get; init; }
	}
}

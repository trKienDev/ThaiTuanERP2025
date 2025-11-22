using ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts;

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

        public sealed record CashoutGroupTreeWithCodesDto
        {
                public Guid Id { get; set; }
                public string Name { get; set; } = null!;
               
                public int Level { get; set; }
                public int OrderNumber { get; set; }
                public string Path { get; set; } = "";

                public List<CashoutGroupTreeWithCodesDto> Children { get; set; } = new();
                public List<CashoutCodeTreeDto> Codes { get; set; } = new();

		public Guid? ParentId { get; set; }
		public string? Description { get; set; }
	}
}

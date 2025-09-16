namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public sealed record DivisionDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string Description { get; init; } = string.Empty;
		public bool IsActive { get; private set; }
		
		public Guid? HeadUserId { get; init; }
		public string? HeadUserName { get; init; }

		public int DepartmentCount { get; init; }
	}

	public sealed record CreateDivisionRequest {
		public string Name { get; init; } = string.Empty;
		public string? Description { get; init; }
		public Guid HeadUserId { get; init; }
	}

	public sealed record UpdateDivisionRequest {
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string? Description { get; init; }
		public Guid? HeadUserId { get; init; }
		public bool IsActive { get; init; } = true;
	}

	public sealed record DivisionSummaryDto {
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string? Description { get; init; }
		public bool IsActive { get; init; }
		public int DepartmentCount { get; init; }
		public string? HeadUserName { get; init;  }
	}
}

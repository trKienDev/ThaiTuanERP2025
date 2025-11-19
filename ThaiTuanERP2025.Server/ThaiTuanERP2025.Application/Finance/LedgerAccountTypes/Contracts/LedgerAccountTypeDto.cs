using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts
{
	public record LedgerAccountTypeDto
	{
		public Guid Id { get; init; }
		public string Code { get; init; } = string.Empty;
		public string Name { get; init; } = string.Empty;
		public LedgerAccountTypeKind LedgerAccountTypeKind { get; init; }
		public string? Description { get; init; } = string.Empty;
		public bool IsActive { get; init; }
	}
}

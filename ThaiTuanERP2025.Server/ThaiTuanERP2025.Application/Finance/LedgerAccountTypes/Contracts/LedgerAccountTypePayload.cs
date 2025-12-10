using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts
{
	public sealed record LedgerAccountTypePayload
	{
		public string Code { get; init; } = string.Empty;
		public string Name { get; init; } = string.Empty;
		public LedgerAccountTypeKind Kind { get; init; }
		public string? Description { get; init; } = string.Empty;
	}
}

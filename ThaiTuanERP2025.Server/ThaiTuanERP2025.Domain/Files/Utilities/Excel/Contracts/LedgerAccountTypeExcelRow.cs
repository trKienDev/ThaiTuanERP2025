namespace ThaiTuanERP2025.Domain.Files.Utilities.Excel.Contracts
{
	public sealed record LedgerAccountTypeExcelRow
	{
		public string Code { get; init; } = string.Empty;
		public string Name { get; init; } = string.Empty;
		public string Kind { get; init; } = string.Empty;
		public string? Description { get; init; }
	}
}

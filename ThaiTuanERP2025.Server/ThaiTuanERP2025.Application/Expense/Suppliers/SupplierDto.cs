namespace ThaiTuanERP2025.Application.Expense.Suppliers
{
	public sealed record SupplierDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = default!;
		public string? TaxCode { get; init; } = string.Empty;
	}
}
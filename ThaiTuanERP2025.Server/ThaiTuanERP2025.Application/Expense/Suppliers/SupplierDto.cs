namespace ThaiTuanERP2025.Application.Expense.Suppliers
{
	public sealed record SupplierDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = default!;
		public string? TaxCode { get; init; } = string.Empty;
	}

	public sealed record SupplierBeneficiaryInforDto
	{
                public string? BeneficiaryAccountNumber { get; init; }
                public string? BeneficiaryName { get; init; }
                public string? BeneficiaryBankName { get; init; }
        }
}
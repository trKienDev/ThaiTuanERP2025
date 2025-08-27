namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record InvoiceLineDto
	{
		public Guid Id { get; init; }
		public string ItemName { get; init; } = null!;
		public string? Unit { get; init; }
		public decimal Quantity { get; init; }
		public decimal UnitPrice { get; init; }
		 public decimal? DiscountRate { get; init; }
		 public decimal? DiscountAmount { get; init; }
		 public decimal NetAmount { get; init; }	
		 public Guid? TaxId { get; init; }
		 public decimal? VATAmount { get; init; }	
		 public Guid? WHTTypeId { get; init; }
		 public decimal? WHTAmount { get; init; }
		 public decimal LineTotal { get; init; }
	}
}

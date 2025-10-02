namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record InvoiceDto
	{
		public Guid Id { get; init; }
		public string InvoiceNumber { get; init; } = default!;
		public string InvoiceName { get; init; } = default!;
		public DateTime IssueDate { get; init; }
		public DateTime? PaymentDate { get; init; }

		// Seller 
		public string? SellerName { get; init; }
		public string SellerTaxCode { get; init; } = default!;
		public string? SellerAddress { get; init; }

		// Buyer
		public string? BuyerName { get; init; }
		public string? BuyerTaxCode { get; init; }
		public string? BuyerAddress { get; init; }	

		// Total
		public decimal TotalAmount { get; set; }
		public decimal TotalTax { get; set; }
		public decimal TotalWithTax { get; set; }

		public bool IsDraft { get; init; }
		
		public IReadOnlyList<InvoiceLineDto> InvoiceLines { get; init; } = Array.Empty<InvoiceLineDto>();

		// File preivews
		public IReadOnlyList<Guid> FileIds { get; init; } = Array.Empty<Guid>();
		public IReadOnlyList<Guid> FollowerUserIds { get; init; } = Array.Empty<Guid>();

		// Totals (computed from lines)
		public decimal SubTotal { get; init; }
		public decimal TotalVAT { get; init; }
		public decimal TotalWHT { get; init; }
		public decimal GrandTotal { get; init; }
	}
}

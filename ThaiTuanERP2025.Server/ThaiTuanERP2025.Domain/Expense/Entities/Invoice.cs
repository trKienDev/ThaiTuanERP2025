using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class Invoice : AuditableEntity
	{
		public string InvoiceNumber { get; set; } = default!;
		public string InvoiceName { get; set; } = default!;
		public DateTime IssueDate { get; set; }
		public DateTime? PaymentDate { get; set; }

		// Seller
		public string? SellerName { get; set; }
		public string SellerTaxCode { get; set; } = default!;
		public string? SellerAddress { get; set; }

		// Buyer
		public string? BuyerName { get; set; }
		public string? BuyerTaxCode { get; set; }
		public string? BuyerAddress { get; set; }

		public bool IsDraft { get; set; } = true;

		// Navigation
		public ICollection<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();
		public ICollection<InvoiceFile> Files { get; set; } = new List<InvoiceFile>();
		public ICollection<InvoiceFollwer> Follwers { get; set; } = new List<InvoiceFollwer>();
	}
}

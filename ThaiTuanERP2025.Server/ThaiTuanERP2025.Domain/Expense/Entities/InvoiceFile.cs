namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class InvoiceFile
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid InvoiceId { get; set; }
		public Guid FileId { get; set; }

		public bool IsMain { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation
		public Invoice Invoice { get; set; } = default!;
	}
}

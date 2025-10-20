using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public record InvoiceFileDto
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid InvoiceId { get; set; }
		public Guid FileId { get; set; }
		public string ObjectKey { get; set; } = null!;
		public bool IsMain { get; set; } = false;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
	
	public sealed record InvoiceFileDetailDto : InvoiceFileDto {
		public Invoice Invoice { get; set; } = default!;
	}
}

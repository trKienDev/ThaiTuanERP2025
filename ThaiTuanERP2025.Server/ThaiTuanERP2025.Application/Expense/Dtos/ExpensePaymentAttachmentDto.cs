namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ExpensePaymentAttachmentDto
	{
		public Guid Id { get; init; }
		public Guid ExpensepaymentId { get; init; }
		public Guid? FileId { get; init; }

		public string ObjectKey { get; init; } = string.Empty;
		public string FileName { get; init; } = string.Empty;
		public long Size { get; init; }	
		public string? Url { get; init; }
	}

	public sealed record ExpensePaymentAttachmentRequest
	{
		public Guid ExpensepaymentId { get; init; }
		public Guid? FileId { get; init; }

		public string ObjectKey { get; init; } = string.Empty;
		public string FileName { get; init; } = string.Empty;
		public long Size { get; init; }
		public string? Url { get; init; }
	}
}

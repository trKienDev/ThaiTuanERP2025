namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ExpensePaymentItemDto
	{
		public Guid Id { get; init; }
		public Guid ExpensePaymentId { get; init; }
		public Guid? InvoiceId { get; init; }
		public Guid? BudgetCodeId { get; init; }	
		public Guid? CashoutCodeId { get; init; }

		public string ItemName { get; init; } = string.Empty;
		public int Quantity { get; init; }
		public decimal UnitPrice { get; init; }
		public decimal TaxRate { get; init; }
		public decimal Amount { get; init; }
		public decimal TaxAmount { get; init; }
		public decimal TotalWithTax { get; init; }
	}

	public sealed record ExpensePaymentItemRequest {
		public Guid? ExpensePaymentId { get; init; }
		public Guid? InvoiceId { get; init; }
		public Guid? BudgetCodeId { get; init; }
		public Guid? CashoutCodeId { get; init; }

		public string ItemName { get; init; } = string.Empty;
		public int Quantity { get; init; }
		public decimal UnitPrice { get; init; }
		public decimal TaxRate { get; init; }
		public decimal Amount { get; init; }
		public decimal TaxAmount { get; init; }
		public decimal TotalWithTax { get; init; }
	}

}

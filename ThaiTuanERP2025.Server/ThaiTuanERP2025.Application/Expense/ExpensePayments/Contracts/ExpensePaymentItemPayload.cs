namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts
{
	public sealed record ExpensePaymentItemPayload
	{
		public string ItemName { get; init; } = default!;
		public Guid? InvoiceStoredFileId { get; init; }
		public Guid BudgetPlanDetailId { get; init; }
		public int Quantity { get; init;  }
		public decimal UnitPrice { get; init; }
		public decimal taxRate { get; init; } 
		public decimal Amount { get; init; }
		public decimal TaxAmount { get; init; } = 0;
		public decimal TotalWithTax { get; init; }
	}
}

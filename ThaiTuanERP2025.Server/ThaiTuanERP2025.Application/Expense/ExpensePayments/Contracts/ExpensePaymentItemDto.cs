using ThaiTuanERP2025.Application.Finance.BudgetCodes;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts
{
	public sealed record ExpensePaymentItemLookupDto
	{
		public string ItemName { get; init; } = string.Empty;
		public int Quantity { get; init; }
		public decimal UnitPrice { get; init; }
		public decimal TaxRate { get; init; }
		public decimal Amount { get; init; }
		public decimal TaxAmount { get; init; }
		public decimal TotalWithTax { get; init; }
		public string CashoutCodeName { get; init; } = string.Empty;

		//public StoredFileMetadataDto? InvoiceFile { get; init; }
		public BudgetCodeLookupDto BudgetCode { get; init; } = new BudgetCodeLookupDto();
	}
}

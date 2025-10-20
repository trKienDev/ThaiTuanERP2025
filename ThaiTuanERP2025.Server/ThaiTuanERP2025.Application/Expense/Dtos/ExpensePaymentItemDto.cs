using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public record ExpensePaymentItemDto
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
	public sealed record ExpensePaymentItemDetailDto : ExpensePaymentItemDto {
		public BudgetCode? BudgetCode { get; init; } = default!;
		public CashoutCode? CashoutCode { get; init; } = default!;
		public InvoiceDto? Invoice { get; init; } = default!;
	}

	public sealed record ExpensePaymentItemRequest {
		public Guid ExpensePaymentId { get; init; }
		public Guid? InvoiceId { get; init; }
		public Guid BudgetCodeId { get; init; }

		public string ItemName { get; init; } = string.Empty;
		public int Quantity { get; init; }
		public decimal UnitPrice { get; init; }
		public decimal TaxRate { get; init; }
		public decimal Amount { get; init; }
		public decimal TaxAmount { get; init; }
		public decimal TotalWithTax { get; init; }
	}


}

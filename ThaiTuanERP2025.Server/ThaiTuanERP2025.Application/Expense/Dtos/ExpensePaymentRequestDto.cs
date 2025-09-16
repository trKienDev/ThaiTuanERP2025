using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class ExpensePaymentItemRequestDto {
		public required string ItemName { get; set; }
		public string? InvoiceId { get; set; }

		public decimal Quantity { get; set; }
		public decimal UnitPrice { get; set; }

		/// <summary>Thuế suất dạng 0.1 = 10%</summary>
		public decimal TaxRate { get; set; }
		
		public decimal Amount { get; set; } // FE tính; BE sẽ tính lại để đảm bảo toàn vẹn
		public decimal TaxAmount { get; set; } // FE gợi ý; BE sẽ tính/kiểm tra 
		public decimal TotalWithTax { get; set; } // Amount + TaxAmount

		public string BudgetCodeId { get; set; } = null!;
		public string CashoutCodeId { get; set; } = null!;
	}
	
	public sealed class ExpensePaymentAttachmentDto {
		public required string ObjectKey { get; set; }
		public string? FileId { get; set; }
		public string? FileName { get; set; }
		public long? Size { get; set; }
		public string? Url { get; set; }
	}

	public sealed class CreateExpensePaymentRequest {
		// Thông tin chung
		public required string Name { get; set; }
		public required PayeeType PayeeType { get; set; }
		public string? SupplierId { get; set; }

		// TK thụ hưởng
		public required string BankName { get; set; }
		public required string AccountNumber { get; set; }
		public required string BeneficiaryName { get; set; }

		// Chi tiết
		public required List<ExpensePaymentItemRequestDto> Items { get; set; }

		// Tổng cộng từ FE (BE sẽ tự tính lại để verify)
		public decimal TotalAmount { get; set; }
		public decimal TotalTax { get; set; }
		public decimal TotalWithTax { get; set; }

		// Phụ
		public List<string>? FollowerIds { get; set; }
		public List<ExpensePaymentAttachmentDto>? Attachments { get; set; }
	}
}

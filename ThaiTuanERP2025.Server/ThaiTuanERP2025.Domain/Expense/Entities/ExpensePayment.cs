using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePayment : AuditableEntity
	{
		public required string Name { get; set; }
		public PayeeType PayeeType { get; set; }
		public Guid? SupplierId { get; set; } // FK to Supplier
		public Guid? UserId { get; set; }

		public required string BankName { get; set; }
		public required string AccountNumber { get; set; }
		public required string BeneficiaryName { get; set; }

		public DateOnly PaymentDate { get; set; }
		public bool HasGoodReceipt { get; set; } = false;

		public decimal TotalAmount { get; set; }
		public decimal TotalTax { get; set; }
		public decimal TotalWithTax { get; set; }

		public List<ExpensePaymentFollower> Followers { get; set; } = new();
	}

	public class ExpensePaymentItem : AuditableEntity {
		public Guid ExpensePaymentID { get; set; }
		public ExpensePayment ExpensePayment { get; set; } = default!;

		public required string ItemName { get; set; }
		public Guid? InvoiceId { get; set; }

		public decimal Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal TaxRate { get; set; }

		public decimal Amount { get; set; }
		public decimal TaxAmount { get; set; }
		public decimal TotalWithTax { get; set; }

		public Guid BudgetCodeId { get; set; }
		public Guid CashoutCodeId { get; set; }
	}

	public class ExpensePaymentAttachment : AuditableEntity {
		public Guid ExpensePaymentId { get; set; }
		public ExpensePayment ExpensePayment { get; set; } = default!;

		public required string ObjectKey { get; set; }
		public Guid? FileId { get; set; }
		public string? FileName { get; set; }
		public long? Size { get; set; }
		public string? Url { get; set; }
	}

	public class ExpensePaymentFollower
	{
		public Guid ExpensePaymentId { get; set; }
		public ExpensePayment ExpensePayment { get; set; } = default!;
		public ICollection<Guid> FollowerIds { get; set; } = new List<Guid>();
	}
}

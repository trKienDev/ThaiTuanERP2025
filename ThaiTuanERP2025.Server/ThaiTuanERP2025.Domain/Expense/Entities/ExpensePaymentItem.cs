using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentItems;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentItem : AuditableEntity
	{
		#region EF Constructor
		private ExpensePaymentItem() { }
		internal ExpensePaymentItem(
			Guid expensePaymentId, string itemName, 
			int quantity, decimal unitPrice, decimal taxRate,
			decimal amount, decimal taxAmount, decimal totalWithTax,
			Guid budgetPlanDetailId, Guid? invoiceFileId
		) {
			Guard.AgainstDefault(expensePaymentId, nameof(expensePaymentId));
			Guard.AgainstNullOrWhiteSpace(itemName, nameof(itemName));
			Guard.AgainstZeroOrNegative(quantity, nameof(quantity));
			Guard.AgainstZeroOrNegative(unitPrice, nameof(unitPrice));
			Guard.AgainstOutOfRange(taxRate, 0, 1, nameof(taxRate));

			Id = Guid.NewGuid();
			ExpensePaymentId = expensePaymentId;
			BudgetPlanDetailId = budgetPlanDetailId;
			ItemName = itemName.Trim();
			Quantity = quantity;
			UnitPrice = unitPrice;
			TaxRate = taxRate;
			Amount = amount;
			TaxAmount = taxAmount;
			TotalWithTax = totalWithTax;
			InvoiceFileId = invoiceFileId;

			AddDomainEvent(new ExpensePaymentItemAddedEvent(this));
		}
		#endregion

		#region Properties
		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public string ItemName { get; private set; } = string.Empty;

		public Guid? InvoiceFileId { get; private set;  }
		public StoredFile? InvoiceFile { get; init; }

		public Guid BudgetPlanDetailId { get; private set; }
		public BudgetPlanDetail BudgetPlanDetail { get; init; } 

		public int Quantity { get; private set; }
		public decimal UnitPrice { get; private set; }
		public decimal TaxRate { get; private set; }

		public decimal Amount { get; private set; }
		public decimal TaxAmount { get; private set; }
		public decimal TotalWithTax { get; private set; }

		#endregion

		#region Domain Behaviors
		#endregion
	}
}

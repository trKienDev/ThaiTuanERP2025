using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentItem : AuditableEntity
	{
		private ExpensePaymentItem() { } // EF

		internal ExpensePaymentItem(Guid paymentId, string itemName, int quantity, decimal unitPrice,
			decimal taxRate, Guid? budgetCodeId, Guid? cashoutCodeId,
			Guid? invoiceId, decimal? overrideTaxAmount
		)
		{
			Id = Guid.NewGuid();
			ExpensePaymentId = paymentId;

			ItemName = itemName.Trim();
			Quantity = quantity;
			UnitPrice = unitPrice;
			TaxRate = taxRate; // 0..1

			BudgetCodeId = budgetCodeId;
			CashoutCodeId = cashoutCodeId;
			InvoiceId = invoiceId;

			Recalculate(overrideTaxAmount);
		}

		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public string ItemName { get; private set; } = string.Empty;

		public Guid? InvoiceId { get; private set; }
		public Invoice? Invoice { get; private set; }

		public int Quantity { get; private set; }            // >=1
		public decimal UnitPrice { get; private set; }       // 18,2
		public decimal TaxRate { get; private set; }         // 0..1 (ví dụ 0.1 = 10%), precision 5,4

		// Tự tính
		public decimal Amount { get; private set; }          // Quantity * UnitPrice
		public decimal TaxAmount { get; private set; }       // gợi ý = Amount * TaxRate (có thể override)
		public decimal TotalWithTax { get; private set; }    // Amount + TaxAmount

		public Guid? BudgetCodeId { get; private set; }
		public BudgetCode? BudgetCode { get; private set; }

		public Guid? CashoutCodeId { get; private set; }
		public CashoutCode? CashoutCode { get; private set; }

		public void UpdateQuantity(int quantity, decimal? overrideTaxAmount = null)
		{
			Quantity = quantity;
			Recalculate(overrideTaxAmount);
		}

		public void UpdateUnitPrice(decimal unitPrice, decimal? overrideTaxAmount = null)
		{
			UnitPrice = unitPrice;
			Recalculate(overrideTaxAmount);
		}

		public void UpdateTaxRate(decimal taxRate, decimal? overrideTaxAmount = null)
		{
			TaxRate = taxRate;
			Recalculate(overrideTaxAmount);
		}

		public void OverrideTaxAmount(decimal taxAmount)
		{
			Recalculate(taxAmount);
		}

		public void LinkInvoice(Guid invoiceId) => InvoiceId = invoiceId;
		public void UnlinkInvoice() => InvoiceId = null;

		public void SetBudgetCode(Guid? id) => BudgetCodeId = id;
		public void SetCashoutCode(Guid? id) => CashoutCodeId = id;

		private void Recalculate(decimal? overrideTaxAmount)
		{
			Amount = Quantity * UnitPrice;

			var suggestedTax = Math.Round(Amount * TaxRate, 0, MidpointRounding.AwayFromZero); // giống UI đang round 0 lẻ
			TaxAmount = overrideTaxAmount ?? suggestedTax;

			TotalWithTax = Amount + TaxAmount;
		}
	}
}

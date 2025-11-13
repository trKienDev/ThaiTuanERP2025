using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentItems;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentItem : AuditableEntity
	{
		#region EF Constructor
		private ExpensePaymentItem() { } 
		internal ExpensePaymentItem(
			Guid expensePaymentId, string itemName, 
			int quantity, decimal unitPrice, decimal taxRate,
			Guid? budgetCodeId, Guid? cashoutCodeId, Guid? invoiceId,
			decimal? overrideTaxAmount = null
		) {
			Guard.AgainstDefault(expensePaymentId, nameof(expensePaymentId));
			Guard.AgainstNullOrWhiteSpace(itemName, nameof(itemName));
			Guard.AgainstZeroOrNegative(quantity, nameof(quantity));
			Guard.AgainstZeroOrNegative(unitPrice, nameof(unitPrice));
			Guard.AgainstOutOfRange(taxRate, 0, 1, nameof(taxRate));

			Id = Guid.NewGuid();
			ExpensePaymentId = expensePaymentId;

			ItemName = itemName.Trim();
			Quantity = quantity;
			UnitPrice = unitPrice;
			TaxRate = taxRate;

			BudgetCodeId = budgetCodeId;
			CashoutCodeId = cashoutCodeId;
			InvoiceId = invoiceId;

			Recalculate(overrideTaxAmount);

			AddDomainEvent(new ExpensePaymentItemAddedEvent(this));
		}
		#endregion

		#region Properties
		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public string ItemName { get; private set; } = string.Empty;

		public Guid? InvoiceId { get; private set; }
		public Invoice? Invoice { get; private set; }

		public int Quantity { get; private set; }
		public decimal UnitPrice { get; private set; }
		public decimal TaxRate { get; private set; }

		public decimal Amount { get; private set; }
		public decimal TaxAmount { get; private set; }
		public decimal TotalWithTax { get; private set; }

		public Guid? BudgetCodeId { get; private set; }
		public BudgetCode? BudgetCode { get; private set; }

		public Guid? CashoutCodeId { get; private set; }
		public CashoutCode? CashoutCode { get; private set; }
		#endregion

		#region Domain Behaviors
		public void UpdateQuantity(int quantity, decimal? overrideTaxAmount = null)
		{
			Guard.AgainstZeroOrNegative(quantity, nameof(quantity));
			Quantity = quantity;
			Recalculate(overrideTaxAmount);
			AddDomainEvent(new ExpensePaymentItemUpdatedEvent(this));
		}

		public void UpdateUnitPrice(decimal unitPrice, decimal? overrideTaxAmount = null)
		{
			Guard.AgainstZeroOrNegative(unitPrice, nameof(unitPrice));
			UnitPrice = unitPrice;
			Recalculate(overrideTaxAmount);
			AddDomainEvent(new ExpensePaymentItemUpdatedEvent(this));
		}

		public void UpdateTaxRate(decimal taxRate, decimal? overrideTaxAmount = null)
		{
			Guard.AgainstOutOfRange(taxRate, 0, 1, nameof(taxRate));
			TaxRate = taxRate;
			Recalculate(overrideTaxAmount);
			AddDomainEvent(new ExpensePaymentItemUpdatedEvent(this));
		}

		public void OverrideTaxAmount(decimal taxAmount)
		{
			Guard.AgainstZeroOrNegative(taxAmount, nameof(taxAmount));
			Recalculate(taxAmount);
			AddDomainEvent(new ExpensePaymentItemUpdatedEvent(this));
		}

		public void LinkInvoice(Guid invoiceId)
		{
			Guard.AgainstDefault(invoiceId, nameof(invoiceId));
			InvoiceId = invoiceId;
			AddDomainEvent(new ExpensePaymentItemLinkedInvoiceEvent(this, invoiceId));
		}

		public void UnlinkInvoice()
		{
			InvoiceId = null;
			AddDomainEvent(new ExpensePaymentItemUnlinkedInvoiceEvent(this));
		}

		public void SetBudgetCode(Guid? id) => BudgetCodeId = id;
		public void SetCashoutCode(Guid? id) => CashoutCodeId = id;

		private void Recalculate(decimal? overrideTaxAmount)
		{
			Amount = Quantity * UnitPrice;

			var suggestedTax = Math.Round(Amount * TaxRate, 0, MidpointRounding.AwayFromZero);
			TaxAmount = overrideTaxAmount ?? suggestedTax;

			TotalWithTax = Amount + TaxAmount;
		}

		#endregion
	}
}

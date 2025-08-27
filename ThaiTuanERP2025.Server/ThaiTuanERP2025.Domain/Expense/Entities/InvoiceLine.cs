using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class InvoiceLine 
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid InvoiceId { get; set; }

		public string? ItemCode { get; set; }
		public string ItemName { get; set; } = default!;
		public string? Unit { get; set; }
		public decimal Quantity { get; set; }
		public decimal UnitPrice { get; set; }	
		public decimal? DiscountRate { get; set; } // %
		public decimal? DiscountAmount { get; set; } // VND
		public decimal? NetAmount { get; set; } // tính tại handler hoặc service

		public Guid? TaxId { get; set; }
		public Tax? Tax { get; set; } // Navigation
		public decimal? VATAmount { get; set; } // tính tại handler hoặc service

		public Guid? WHTTypeId { get; set; }
		public WithholdingTaxType? WHTType { get; set; }
		public decimal? WHTAmount { get; set; }

		public decimal LineTotal { get; set; }

		// Navigation
		public Invoice Invoice { get; set; } = default!;
	}
}

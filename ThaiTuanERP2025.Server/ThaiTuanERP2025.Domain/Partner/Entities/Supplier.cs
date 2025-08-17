using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Partner.Entities
{
	public class Supplier : AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;	
		public string? ShortName { get; set; }
		public bool IsActive { get; set; } = true; // Default to true, can be set to false for soft delete

		public string? TaxCode { get; set; }
		public string? WithholdingTaxType { get; set; } 
		public decimal? WithholdingTaxRate { get; set; }
		public string DefaultCurrency { get; set; } = "VND";
		public int PaymentTermDays { get; set; } = 30;

		public Guid? PostingProfileId { get; set; }
		public Guid? SupplierGroupId { get; set; }

		public string? Email { get; set; }
		public string? Phone { get; set; }	
		public string? AddressLine1 { get; set; }
		public string? AddressLine2 { get; set; }
		public string? City { get; set; }
		public string? StateOrProvince { get; set; }
		public string? PostalCode { get; set; }
		public string? Country { get; set; }	
		public string? Note { get; set; }

		public PartnerBankAccount? BankAccount { get; set; }

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }
	}
}

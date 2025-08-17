using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Partner.Entities
{
	public class PartnerBankAccount : AuditableEntity
	{
		public Guid SupplierId { get; set; }            // 1-1 với Supplier (unique)
		public string AccountNumber { get; set; } = null!;
		public string BankName { get; set; } = null!;
		public string? AccountHolder { get; set; }
		public string? SwiftCode { get; set; }
		public string? Branch { get; set; }
		public string? Note { get; set; }
		public bool IsActive { get; set; } = true;

		public Supplier Supplier { get; set; } = null!;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }
	}
}

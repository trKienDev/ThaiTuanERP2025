using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class BankAccount : AuditableEntity
	{
		public string BankName { get; set; } = null!;
		public string AccountNumber { get; set; } = null!;
		public string BeneficiaryName { get; set; } = null!; // Tên người thụ hưởng

		// Owner information
		public Guid? UserId { get; set; }
		public User? User { get; set; }

		public Guid? SupplierId {  get; set; }
		public Supplier? Supplier { get; set; }

		public bool IsActive { get; set; } = true;
	}
}

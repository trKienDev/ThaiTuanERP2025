using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BankAccount : AuditableEntity
	{
		public string AccountNumber { get; set; } = null!;
		public string BankName { get; set; } = null!;
		public string AccountHolder { get; set; } = null!;
		public string? EmployeeCode { get; set; }
		public string? CustomerName { get; set; }
		public string? Note { get; set; }
		public bool IsActive { get; set; } = true;

		public Guid? DepartmentId { get; set; }

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

	}
}

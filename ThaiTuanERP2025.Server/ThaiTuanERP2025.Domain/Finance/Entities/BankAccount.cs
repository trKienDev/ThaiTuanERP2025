using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BankAccount
	{
		public Guid Id { get; set; }
		public string AccountNumber { get; set; } = null!;
		public string BankName { get; set; } = null!;
		public string AccountHolder { get; set; } = null!;
		public string? EmployeeCode { get; set; }

		public Guid? DepartmentId {  get; set; }
		public string? CustomerName { get; set; }

		public string? Note { get; set; }
		public bool IsActive { get; set; } = true;
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	}
}

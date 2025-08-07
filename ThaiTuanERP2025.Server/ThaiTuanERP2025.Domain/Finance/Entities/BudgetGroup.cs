using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetGroup
	{
		public Guid Id { get; set; }
		public string Code	{ get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		public Guid CreatedByUserId { get; set; } // Foreign Key
		public User CreatedByUser { get; set; } = default!; // Navigation property

		public DateTime? DateModified { get; set; }
		public Guid? ModifiedByUserId { get; set; }
		public User? ModifiedByUser { get; set; }

		public ICollection<BudgetCode> BudgetCodes { get; set; } = new List<BudgetCode>();
	}
}

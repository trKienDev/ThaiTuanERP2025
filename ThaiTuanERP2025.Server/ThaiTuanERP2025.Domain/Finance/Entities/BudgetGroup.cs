using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetGroup : AuditableEntity
	{
		public string Code	{ get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;

		public User CreatedByUser { get; set; } = default!; // Navigation property
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; } // Navigation property for soft-delete

		public ICollection<BudgetCode> BudgetCodes { get; set; } = new List<BudgetCode>();
	}
}

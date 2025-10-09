using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPeriod : AuditableEntity
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public bool IsActive { get; set; } = true;

		public ICollection<BudgetPlan> BudgetPlans { get; set; } = new List<BudgetPlan>();
	}
}

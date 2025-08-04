using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPeriod
	{
		public Guid Id { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedDate { get; set; }
		public bool IsActive { get; set; } = true;
		public ICollection<BudgetPlan> BudgetPlans { get; set; } = new List<BudgetPlan>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetCode
	{
		public Guid Id { get; set; }
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public Guid BudgetGroupId { get; set; }
		public BudgetGroup BudgetGroup { get; set; } = null!;
		public bool IsActive { get; set; } = true;
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		
		public ICollection<BudgetPlan> BudgetPlans { get; set; } = new List<BudgetPlan>();
	}
}

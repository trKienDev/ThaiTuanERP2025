using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPlan
	{
		public Guid Id { get; set; }
		public Guid DepartmentId { get; set; }
		public Guid BudgetCodeId { get; set; }
		public Guid BudgetPeriodId { get; set; }
		public decimal Amount {  get; set; }
		public string Status { get; set; } = "Draft";
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public BudgetCode BudgetCode { get; set; } = null!;
		public BudgetPeriod BudgetPeriod { get; set; } = null!;
	}
}

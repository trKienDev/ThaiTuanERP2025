using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public class BudgetPlanDto
	{
		public Guid Id { get; set; }
		public Guid DepartmentId { get; set; }
		public Guid BudgetCodeId { get; set; }
		public Guid BudgetPeriodId { get; set; }
		public decimal Amount { get; set; }
		public string Status { get; set; } = "Draft";

		public string DepartmentName { get; set; } = string.Empty;
		public string BudgetCodeName { get; set; } = string.Empty;
		public string BudgetPeriodName { get; set; } = string.Empty;
	}
}

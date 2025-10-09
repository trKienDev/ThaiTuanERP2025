using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPlan : AuditableEntity
	{
		public Guid DepartmentId { get; set; }
		public Guid BudgetCodeId { get; set; }
		public Guid BudgetPeriodId { get; set; }
		public decimal Amount {  get; set; }
		public string Status { get; set; } = "Draft";

		public BudgetCode BudgetCode { get; set; } = null!;
		public BudgetPeriod BudgetPeriod { get; set; } = null!;
		public Department Department { get; set; } = null!;
	}
}

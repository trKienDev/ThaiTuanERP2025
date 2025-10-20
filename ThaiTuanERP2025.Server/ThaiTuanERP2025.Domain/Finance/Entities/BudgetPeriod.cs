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

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }
	}
}

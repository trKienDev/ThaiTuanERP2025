using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPeriod : AuditableEntity
	{
		private BudgetPeriod( ) { }
		public BudgetPeriod (int year, int month, DateTime budgetPreparationDate) {
			this.Year = year;
			this.Month = month;
			this.BudgetPreparationDate = budgetPreparationDate;
			this.IsActive = true;
		}

		public int Year { get; private set; }
		public int Month { get; private set; }
		public DateTime BudgetPreparationDate { get; private set; }
		public bool IsActive { get; private set; } = true;

		public ICollection<BudgetPlan> BudgetPlans { get; set; } = new List<BudgetPlan>();

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public void DeactiveBudgetPeriod() => IsActive = false;
		public void ActivateBudgetPeriod() => IsActive = true;
	}
}

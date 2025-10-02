using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetCode : AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public Guid BudgetGroupId { get; set; }
		public bool IsActive { get; set; } = true;

		public BudgetGroup BudgetGroup { get; set; } = null!;

		public Guid CashoutCodeId { get; set; }
		public CashoutCode CashoutCode { get; set; } = null!;

		public User CreatedByUser { get; set; } = default!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; } 

		public ICollection<BudgetPlan> BudgetPlans { get; set; } = new List<BudgetPlan>();
	}
}

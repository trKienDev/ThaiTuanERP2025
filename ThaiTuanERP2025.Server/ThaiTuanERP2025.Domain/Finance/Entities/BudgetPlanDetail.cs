using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPlanDetail : BaseEntity
	{
		#region Constructors
		private BudgetPlanDetail() { }
		internal BudgetPlanDetail(Guid planId, Guid budgetCodeId, decimal amount, Guid userId)
		{
			BudgetPlanId = planId;
			BudgetCodeId = budgetCodeId;
			Amount = amount;
			IsActive = true;

			ModifiedAt = DateTime.UtcNow;
			ModifiedByUserId = userId;
		}
		#endregion

		#region Properties
		public Guid BudgetPlanId { get; private set; }
		public Guid BudgetCodeId { get; private set; }
		public decimal Amount { get; private set; }
		public bool IsActive { get; private set; }

		public DateTime ModifiedAt { get; private set; }
		public Guid ModifiedByUserId { get; private set; }
		public User ModifiedByUser { get; private set; } = null!;

		public DateTime? DeletedAt { get; private set; }
		public Guid? DeletedByUserId { get; private set; }
		public User? DeletedByUser { get; private set; }
		#endregion

		#region Behaviors
		internal void UpdateAmount(decimal amount, Guid userId)
		{
			Amount = amount;
			ModifiedAt = DateTime.UtcNow;
			ModifiedByUserId = userId;
		}

		internal void SoftDelete(Guid userId)
		{
			IsActive = false;
			DeletedAt = DateTime.UtcNow;
			DeletedByUserId = userId;
		}
		#endregion
	}
}

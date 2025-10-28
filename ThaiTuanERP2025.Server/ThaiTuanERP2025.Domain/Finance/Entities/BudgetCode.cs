using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetCode : AuditableEntity
	{
		private BudgetCode() { }
		public BudgetCode(string code, string name, Guid budgetGroupId, Guid cashoutCodeId) {
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Tên mã ngân sách không hợp lệ", nameof(name));

			if (string.IsNullOrWhiteSpace(code))
				throw new ArgumentException("mã ngân sách không hợp lệ", nameof(code));

			Id = Guid.NewGuid();
			Name = name;
			Code = code;
			BudgetGroupId = budgetGroupId;
			CashoutCodeId = cashoutCodeId;
		} 

		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public Guid BudgetGroupId { get; set; }
		public bool IsActive { get; set; } = true;

		public BudgetGroup BudgetGroup { get; set; } = null!;

		public Guid CashoutCodeId { get; set; }
		public CashoutCode CashoutCode { get; set; } = null!;

		public ICollection<BudgetPlan> BudgetPlans { get; set; } = new List<BudgetPlan>();

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public void Deactivate() => IsActive = false;
		public void Activate() => IsActive = true;
	}
}

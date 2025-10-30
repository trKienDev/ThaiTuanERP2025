using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPeriod : AuditableEntity
	{
		private BudgetPeriod( ) { }
		public BudgetPeriod (int year, int month, DateTime startDate, DateTime endDate) {
			this.Year = year;
			this.Month = month;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.IsActive = true;
		}

		public int Year { get; private set; }
		public int Month { get; private set; }
		public DateTime StartDate { get; private set; }
		public DateTime EndDate { get; private set; }
		public bool IsActive { get; private set; } = true;

		public ICollection<BudgetPlan> BudgetPlans { get; set; } = new List<BudgetPlan>();

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public void AddBudgetPlan(BudgetPlan plan)
		{
			if (plan == null)
				throw new ArgumentNullException(nameof(plan));

			// Đảm bảo BudgetPlan phải thuộc cùng BudgetPeriod
			if (plan.BudgetPeriodId != this.Id)
				plan.AssignToPeriod(this.Id);

			// (Tùy chọn) kiểm tra trùng Department + BudgetCode
			bool exists = BudgetPlans.Any(
				x => x.DepartmentId == plan.DepartmentId && x.BudgetCodeId == plan.BudgetCodeId
			);

			if (exists)
				throw new InvalidOperationException(
				    $"Phòng ban đã có kế hoạch ngân sách cho mã ngày sách trong kỳ này.");

			BudgetPlans.Add(plan);
		}

		public void RemoveBudgetPlan(Guid budgetPlanId)
		{
			var plan = BudgetPlans.FirstOrDefault(x => x.Id == budgetPlanId)
			    ?? throw new KeyNotFoundException("Không tìm thấy kế hoạch ngân sách.");

			BudgetPlans.Remove(plan);
		}

		public void SetStartDate(DateTime startDate) {
			if (startDate > EndDate)
				throw new InvalidOperationException("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
			StartDate = startDate;
		}
		public void SetEndDate(DateTime endDate) {
			if (endDate < StartDate)
				throw new InvalidOperationException("Ngày kết thúc không thể trước ngày bắt đầu.");
			EndDate = endDate;
		}

		public void DeactiveBudgetPeriod() => IsActive = false;
		public void ActivateBudgetPeriod() => IsActive = true;
	}
}

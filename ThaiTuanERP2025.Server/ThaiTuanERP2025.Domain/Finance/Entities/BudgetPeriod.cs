using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPeriod : AuditableEntity
	{
		private BudgetPeriod() { }

		public BudgetPeriod(int year, int month, DateTime startDate, DateTime endDate)
		{
			Guard.AgainstOutOfRange(year, 2000, 2100, nameof(year));
			Guard.AgainstOutOfRange(month, 1, 12, nameof(month));
			Guard.AgainstInvalidDateRange(startDate, endDate, nameof(BudgetPeriod));

			Id = Guid.NewGuid();
			Year = year;
			Month = month;
			StartDate = startDate;
			EndDate = endDate;
			IsActive = true;

			AddDomainEvent(new BudgetPeriodCreatedEvent(this));
		}

		public int Year { get; private set; }
		public int Month { get; private set; }
		public DateTime StartDate { get; private set; }
		public DateTime EndDate { get; private set; }
		public bool IsActive { get; private set; }

		public ICollection<BudgetPlan> BudgetPlans { get; private set; } = new List<BudgetPlan>();

		public User CreatedByUser { get; private set; } = null!;
		public User? ModifiedByUser { get; private set; }
		public User? DeletedByUser { get; private set; }

		public void AddBudgetPlan(BudgetPlan plan)
		{
			Guard.AgainstNull(plan, nameof(plan));

			if (plan.BudgetPeriodId != Id)
				plan.AssignToPeriod(Id);

			bool exists = BudgetPlans.Any(
				x => x.DepartmentId == plan.DepartmentId && x.BudgetCodeId == plan.BudgetCodeId
			);

			if (exists)
				throw new DomainException("Phòng ban đã có kế hoạch ngân sách cho mã ngân sách trong kỳ này.");

			BudgetPlans.Add(plan);
			AddDomainEvent(new BudgetPlanAddedToPeriodEvent(this, plan));
		}

		public void RemoveBudgetPlan(Guid budgetPlanId)
		{
			var plan = BudgetPlans.FirstOrDefault(x => x.Id == budgetPlanId)
			    ?? throw new KeyNotFoundException("Không tìm thấy kế hoạch ngân sách.");

			BudgetPlans.Remove(plan);
			AddDomainEvent(new BudgetPlanRemovedFromPeriodEvent(this, plan));
		}

		public void SetStartDate(DateTime startDate)
		{
			if (startDate > EndDate)
				throw new DomainException("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
			StartDate = startDate;
			AddDomainEvent(new BudgetPeriodUpdatedEvent(this));
		}

		public void SetEndDate(DateTime endDate)
		{
			if (endDate < StartDate)
				throw new DomainException("Ngày kết thúc không thể trước ngày bắt đầu.");
			EndDate = endDate;
			AddDomainEvent(new BudgetPeriodUpdatedEvent(this));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new BudgetPeriodActivatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new BudgetPeriodDeactivatedEvent(this));
		}
	}

}

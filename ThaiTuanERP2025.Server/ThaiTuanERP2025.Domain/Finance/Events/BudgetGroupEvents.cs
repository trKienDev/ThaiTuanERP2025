using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Events
{
	public abstract class BudgetPeriodEventBase : IDomainEvent
	{
		public Guid BudgetPeriodId { get; }
		public DateTime OccurredOn { get; }
		protected BudgetPeriodEventBase(Guid budgetGroupId)
		{
			BudgetPeriodId = budgetGroupId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class BudgetPeriodActivatedEvent : BudgetPeriodEventBase
	{
		public BudgetPeriodActivatedEvent(BudgetPeriod budgetPeriod) : base(budgetPeriod.Id)
		{
			BudgetPeriod = budgetPeriod;
		}
		public BudgetPeriod BudgetPeriod { get; }
	}

	public sealed class BudgetPeriodCreatedEvent : BudgetPeriodEventBase
	{
		public BudgetPeriodCreatedEvent(BudgetPeriod budgetPeriod) : base(budgetPeriod.Id)
		{
			BudgetPeriod = budgetPeriod;
		}
		public BudgetPeriod BudgetPeriod { get; }
	}

	public sealed class BudgetPeriodDeactivatedEvent : BudgetPeriodEventBase
	{
		public BudgetPeriodDeactivatedEvent(BudgetPeriod budgetPeriod) : base(budgetPeriod.Id)
		{
			BudgetPeriod = budgetPeriod;
		}
		public BudgetPeriod BudgetPeriod { get; }
	}

	public sealed class BudgetPeriodUpdatedEvent : BudgetPeriodEventBase
	{
		public BudgetPeriodUpdatedEvent(BudgetPeriod budgetPeriod) : base(budgetPeriod.Id)
		{
			BudgetPeriod = budgetPeriod;
		}
		public BudgetPeriod BudgetPeriod { get; }
	}

	public sealed class BudgetPlanAddedToPeriodEvent : BudgetPeriodEventBase
	{
		public BudgetPlanAddedToPeriodEvent(BudgetPeriod budgetPeriod, BudgetPlan budgetPlan) : base(budgetPeriod.Id)
		{
			BudgetPeriod = budgetPeriod;
			BudgetPlan = budgetPlan;
		}

		public BudgetPeriod BudgetPeriod { get; }
		public BudgetPlan BudgetPlan { get; }
	}

	public sealed class BudgetPlanRemovedFromPeriodEvent : BudgetPeriodEventBase
	{
		public BudgetPlanRemovedFromPeriodEvent(BudgetPeriod budgetPeriod, BudgetPlan budgetPlan) : base(budgetPeriod.Id)
		{
			BudgetPeriod = budgetPeriod;
			BudgetPlan = budgetPlan;
		}

		public BudgetPeriod BudgetPeriod { get; }
		public BudgetPlan BudgetPlan { get; }
	}

}

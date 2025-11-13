using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Finance.Events
{
	public sealed class BudgetApproverChangedEvent : IDomainEvent
	{
		public BudgetApproverChangedEvent(Guid newBudgetApproverId, Guid newApproverUserId, int newSlaHours)
		{
			BudgetApproverId = newBudgetApproverId;
			NewApproverUserId = newApproverUserId;
			NewHours = newSlaHours;
			OccurredOn = DateTime.UtcNow;
		}

		public Guid BudgetApproverId { get; }
		public Guid NewApproverUserId { get; }
		public int NewHours{ get; }
		public DateTime OccurredOn { get; }
	}
}

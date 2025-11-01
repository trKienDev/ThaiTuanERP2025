using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Events
{
	public sealed class BudgetPlanCreatedEvent : IDomainEvent
	{
		public BudgetPlan BudgetPlan { get; }
		public DateTime OccurredOn { get; }

		public BudgetPlanCreatedEvent(BudgetPlan plan)
		{
			BudgetPlan = plan;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class BudgetPlanReviewedEvent : IDomainEvent
	{
		public Guid BudgetPlanId { get; }
		public Guid ReviewedByUserId { get; }
		public DateTime OccurredOn { get; }
		public BudgetPlanReviewedEvent(Guid planId, Guid userId)
		{
			BudgetPlanId = planId;
			ReviewedByUserId = userId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class BudgetPlanApprovedEvent : IDomainEvent
	{
		public Guid BudgetPlanId { get; }
		public Guid ApprovedByUserId { get; }
		public DateTime OccurredOn { get; }

		public BudgetPlanApprovedEvent(Guid planId, Guid userId)
		{
			BudgetPlanId = planId;
			ApprovedByUserId = userId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class BudgetPlanRejectedEvent : IDomainEvent
	{
		public Guid BudgetPlanId { get; }
		public Guid RejectedByUserId { get; }
		public DateTime OccurredOn { get; }

		public BudgetPlanRejectedEvent(Guid planId, Guid userId)
		{
			BudgetPlanId = planId;
			RejectedByUserId = userId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class BudgetPlanTransactionRecordedEvent : IDomainEvent
	{
		public Guid BudgetPlanId { get; }
		public Guid TransactionId { get; }
		public decimal Amount { get; }
		public BudgetTransactionType Type { get; }
		public DateTime OccurredOn { get; }

	public BudgetPlanTransactionRecordedEvent(Guid planId, Guid transactionId, decimal amount, BudgetTransactionType type)
		{
			BudgetPlanId = planId;
			TransactionId = transactionId;
			Amount = amount;
			Type = type;
			OccurredOn = DateTime.UtcNow;
		}
	}
}

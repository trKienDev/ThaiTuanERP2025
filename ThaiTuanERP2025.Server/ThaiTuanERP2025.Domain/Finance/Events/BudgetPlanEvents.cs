using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Events
{
	public abstract class BudgetPlanEventBase : IDomainEvent
	{
		public Guid BudgetPlanId { get; }
		public DateTime OccurredOn { get; }

		protected BudgetPlanEventBase(Guid planId)
		{
			BudgetPlanId = planId;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class BudgetPlanCreatedEvent : BudgetPlanEventBase
	{
		public BudgetPlan BudgetPlan { get; }
		public Guid ReviewerUserId { get; }
		public DateTime DateCreated { get; }
		public BudgetPlanCreatedEvent(BudgetPlan plan, Guid reviewerUserId, DateTime dateCreated) : base(plan.Id)
		{
			BudgetPlan = plan;
			ReviewerUserId = reviewerUserId;
			DateCreated = dateCreated;
		}
	}

	public abstract class BudgetPlanUserActionEvent : BudgetPlanEventBase
	{
		public Guid UserId { get; }
		protected BudgetPlanUserActionEvent(Guid planId, Guid userId) : base(planId)
		{
			UserId = userId;
		}
	}

	public sealed class BudgetPlanReviewedEvent : BudgetPlanUserActionEvent
	{
		public BudgetPlanReviewedEvent(Guid planId, Guid userId) : base(planId, userId) { }
	}

	public sealed class BudgetPlanApprovedEvent : BudgetPlanUserActionEvent
	{
		public BudgetPlanApprovedEvent(Guid planId, Guid userId) : base(planId, userId) { }
	}

	public class BudgetPlanAssignedForApprovalEvent : BudgetPlanEventBase {
		public BudgetPlanAssignedForApprovalEvent(Guid planId, Guid approverByUserId, DateTime approvalDeadline) : base(planId) {
			BudgetPlanId = planId;
			ApproverByUserId = approverByUserId;
			ApprovalDeadline = approvalDeadline;
		}

		public Guid BudgetPlanId { get; }
		public Guid ApproverByUserId { get; }	
		public DateTime ApprovalDeadline { get; }

	}

	public sealed class BudgetPlanRejectedEvent : BudgetPlanUserActionEvent
	{
		public BudgetPlanRejectedEvent(Guid planId, Guid userId) : base(planId, userId) { }
	}

	public sealed class BudgetPlanTransactionRecordedEvent : BudgetPlanEventBase
	{
		public Guid TransactionId { get; }
		public decimal Amount { get; }
		public BudgetTransactionType Type { get; }

		public BudgetPlanTransactionRecordedEvent(Guid planId, Guid transactionId, decimal amount, BudgetTransactionType type) : base(planId)
		{
			TransactionId = transactionId;
			Amount = amount;
			Type = type;
		}
	}

}

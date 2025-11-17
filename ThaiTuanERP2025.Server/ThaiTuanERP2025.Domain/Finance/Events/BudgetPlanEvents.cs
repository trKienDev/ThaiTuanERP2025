using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Events
{
	public abstract class BudgetPlanEventBase : IDomainEvent
	{
		public BudgetPlan BudgetPlan { get; }
		public DateTime OccurredOn { get; }

		protected BudgetPlanEventBase(BudgetPlan plan)
		{
			BudgetPlan = plan;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class BudgetPlanCreatedEvent : BudgetPlanEventBase
	{
		public Guid ReviewerUserId { get; }
		public DateTime DueAt { get; }
		public BudgetPlanCreatedEvent(BudgetPlan plan, Guid reviewerUserId, DateTime dueAt) : base(plan)
		{
			ReviewerUserId = reviewerUserId;
			DueAt = dueAt;
		}
	}

	public sealed class BudgetPlanReviewedEvent : BudgetPlanEventBase
	{
		public Guid ApproverUserId { get; }
		public DateTime DueAt { get; }
		public BudgetPlanReviewedEvent(BudgetPlan plan, Guid approverUserId, DateTime dueAt) : base(plan)
		{
			ApproverUserId = approverUserId;
			DueAt = dueAt;
		}
	}

	//public sealed class BudgetPlanApprovedEvent : BudgetPlanUserActionEvent
	//{
	//	public BudgetPlanApprovedEvent(Guid planId, Guid userId) : base(planId, userId) { }
	//}

	//public class BudgetPlanAssignedForApprovalEvent : BudgetPlanEventBase {
	//	public BudgetPlanAssignedForApprovalEvent(Guid planId, Guid approverByUserId, DateTime approvalDeadline) : base(planId) {
	//		BudgetPlanId = planId;
	//		ApproverByUserId = approverByUserId;
	//		ApprovalDeadline = approvalDeadline;
	//	}

	//	public Guid BudgetPlanId { get; }
	//	public Guid ApproverByUserId { get; }	
	//	public DateTime ApprovalDeadline { get; }

	//}

	//public sealed class BudgetPlanRejectedEvent : BudgetPlanUserActionEvent
	//{
	//	public BudgetPlanRejectedEvent(Guid planId, Guid userId) : base(planId, userId) { }
	//}

	//public sealed class BudgetPlanTransactionRecordedEvent : BudgetPlanEventBase
	//{
	//	public Guid TransactionId { get; }
	//	public decimal Amount { get; }
	//	public BudgetTransactionType Type { get; }

	//	public BudgetPlanTransactionRecordedEvent(Guid planId, Guid transactionId, decimal amount, BudgetTransactionType type) : base(planId)
	//	{
	//		TransactionId = transactionId;
	//		Amount = amount;
	//		Type = type;
	//	}
	//}

}

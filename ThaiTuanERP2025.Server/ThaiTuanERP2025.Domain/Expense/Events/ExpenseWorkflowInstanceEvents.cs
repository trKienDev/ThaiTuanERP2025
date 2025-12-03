using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Domain.Expense.Events
{
	public abstract class ExpenseWorkflowInstanceEventBase : IDomainEvent
	{
		public ExpenseWorkflowInstance WorkflowInstance { get; }
		public DateTime OccurredOn { get; }
		protected ExpenseWorkflowInstanceEventBase(ExpenseWorkflowInstance workflowInstance) {
			WorkflowInstance = workflowInstance;
			OccurredOn = DateTime.UtcNow;
		}
	}

	public sealed class ExpenseWorkflowInstanceCreatedEvent : ExpenseWorkflowInstanceEventBase
	{
		public ExpenseWorkflowInstanceCreatedEvent(ExpenseWorkflowInstance workflowInstance) : base(workflowInstance) { }
	}

	public sealed class ExpenseWorkflowInstanceStartedEvent : ExpenseWorkflowInstanceEventBase
	{
		public ExpenseWorkflowInstanceStartedEvent(ExpenseWorkflowInstance workflowInstance) : base(workflowInstance) { }
	}

	public sealed class ExpenseWorkflowInstanceApprovedEvent : ExpenseWorkflowInstanceEventBase
	{
		public Guid DocumentId { get; }
		public DocumentType DocumentType { get; }

		public ExpenseWorkflowInstanceApprovedEvent(ExpenseWorkflowInstance workflowInstance, Guid documentId, DocumentType documentType) : base(workflowInstance) {
			DocumentId = documentId;
			DocumentType = documentType;
		}
	}
}

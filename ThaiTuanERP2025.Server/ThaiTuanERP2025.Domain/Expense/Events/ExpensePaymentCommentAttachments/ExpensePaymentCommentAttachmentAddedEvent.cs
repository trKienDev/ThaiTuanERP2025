using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentCommentAttachments
{
	public sealed class ExpensePaymentCommentAttachmentAddedEvent : IDomainEvent
	{
		public ExpensePaymentCommentAttachmentAddedEvent(ExpensePaymentCommentAttachment attachment)
		{
			Attachment = attachment;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentCommentAttachment Attachment { get; }
		public DateTime OccurredOn { get; }
	}
}

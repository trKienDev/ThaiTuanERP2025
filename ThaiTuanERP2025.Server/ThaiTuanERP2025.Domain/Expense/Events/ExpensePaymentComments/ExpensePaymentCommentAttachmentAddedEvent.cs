using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentComments
{
	public sealed class ExpensePaymentCommentAttachmentAddedEvent : IDomainEvent
	{
		public ExpensePaymentCommentAttachmentAddedEvent(
		    ExpensePaymentComment comment,
		    ExpensePaymentCommentAttachment attachment)
		{
			Comment = comment;
			Attachment = attachment;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentComment Comment { get; }
		public ExpensePaymentCommentAttachment Attachment { get; }
		public DateTime OccurredOn { get; }
	}
}

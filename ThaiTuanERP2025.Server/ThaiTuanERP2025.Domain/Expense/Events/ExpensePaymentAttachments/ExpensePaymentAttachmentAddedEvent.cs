using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentAttachments
{
	public sealed class ExpensePaymentAttachmentAddedEvent : IDomainEvent
	{
		public ExpensePaymentAttachmentAddedEvent(ExpensePaymentAttachment attachment)
		{
			Attachment = attachment;
			OccurredOn = DateTime.UtcNow;
		}

		public ExpensePaymentAttachment Attachment { get; }
		public DateTime OccurredOn { get; }
	}
}

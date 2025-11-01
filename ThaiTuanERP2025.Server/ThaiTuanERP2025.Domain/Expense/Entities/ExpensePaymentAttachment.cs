using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Expense.Events.ExpensePaymentAttachments;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentAttachment : AuditableEntity
	{
		private ExpensePaymentAttachment() { } // EF Core
		public ExpensePaymentAttachment(
			Guid expensePaymentId, string objectKey, string fileName,
			long size, string? url = null, Guid? fileId = null
		) {
			Guard.AgainstDefault(expensePaymentId, nameof(expensePaymentId));
			Guard.AgainstNullOrWhiteSpace(objectKey, nameof(objectKey));
			Guard.AgainstNullOrWhiteSpace(fileName, nameof(fileName));
			Guard.AgainstNegativeOrZero(size, nameof(size));

			Id = Guid.NewGuid();
			ExpensePaymentId = expensePaymentId;
			ObjectKey = objectKey.Trim();
			FileName = fileName.Trim();
			Size = size;
			Url = url;
			FileId = fileId;

			AddDomainEvent(new ExpensePaymentAttachmentAddedEvent(this));
		}

		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public string ObjectKey { get; private set; } = null!;
		public Guid? FileId { get; private set; }
		public string FileName { get; private set; } = null!;
		public long Size { get; private set; }
		public string? Url { get; private set; }
	}
}

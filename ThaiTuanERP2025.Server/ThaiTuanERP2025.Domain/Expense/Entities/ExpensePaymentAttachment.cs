using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentAttachment : AuditableEntity
	{
		private ExpensePaymentAttachment() { }

		public ExpensePaymentAttachment(Guid paymentId, string objectKey, string fileName, long size, string? url = null, Guid? fileId = null)
		{
			Id = Guid.NewGuid();
			ExpensePaymentId = paymentId;
			ObjectKey = objectKey;
			FileId = fileId;
			FileName = fileName;
			Size = size;
			Url = url;
		}

		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public string ObjectKey { get; private set; } = string.Empty; // lưu object key (ưu tiên)
		public Guid? FileId { get; private set; }                     // hoặc Id file nếu có
		public string FileName { get; private set; } = string.Empty;
		public long Size { get; private set; }                        // bytes
		public string? Url { get; private set; }
	}
}

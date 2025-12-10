using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePaymentAttachment : AuditableEntity
	{
		#region EF Constructor	
		private ExpensePaymentAttachment() { } // EF Core
		public ExpensePaymentAttachment(Guid expensePaymentId, Guid storedFileId)
		{
			Guard.AgainstDefault(expensePaymentId, nameof(expensePaymentId));
			Guard.AgainstDefault(storedFileId, nameof(storedFileId));

			Id = Guid.NewGuid();
			ExpensePaymentId = expensePaymentId;
			StoredFileId = storedFileId;
		}
		#endregion

		#region Properties
		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public Guid StoredFileId { get; private set; }
		public StoredFile StoredFile { get; private set; } = null!;
		#endregion
	}
}

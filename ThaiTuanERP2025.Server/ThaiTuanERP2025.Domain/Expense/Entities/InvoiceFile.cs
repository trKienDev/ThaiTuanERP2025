using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Expense.Events.InvoiceFiles;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class InvoiceFile : AuditableEntity
	{
		#region Properties
		public Guid InvoiceId { get; private set; }
		public Guid FileId { get; private set; }
		public bool IsMain { get; private set; }
		public DateTime CreatedAt { get; private set; }

		public Invoice Invoice { get; private set; } = default!;
		public StoredFile File { get; private set; } = default!;
		#endregion

		#region Constructor
		private InvoiceFile() { }
		public InvoiceFile(Guid invoiceId, Guid fileId, bool isMain = false)
		{
			Guard.AgainstDefault(invoiceId, nameof(invoiceId));
			Guard.AgainstDefault(fileId, nameof(fileId));

			Id = Guid.NewGuid();
			InvoiceId = invoiceId;
			FileId = fileId;
			IsMain = isMain;
			CreatedAt = DateTime.UtcNow;
		}
		#endregion

		#region Domain Behaviors
		public void MarkAsMain()
		{
			if (IsMain) return;

			IsMain = true;
			AddDomainEvent(new InvoiceFileMarkedAsMainEvent(this));
		}

		public void UnmarkMain()
		{
			IsMain = false;
		}
		#endregion
	}
}

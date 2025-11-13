using ThaiTuanERP2025.Domain.Shared.Events;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Domain.Files.Events.StoredFiles
{
	public sealed class StoredFileMadePublicEvent : IDomainEvent
	{
		public StoredFileMadePublicEvent(StoredFile file)
		{
			File = file;
			OccurredOn = DateTime.UtcNow;
		}

		public StoredFile File { get; }
		public DateTime OccurredOn { get; }
	}
}

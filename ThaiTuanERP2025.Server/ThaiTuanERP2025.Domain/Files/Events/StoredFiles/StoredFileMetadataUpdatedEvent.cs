using ThaiTuanERP2025.Domain.Common.Events;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Domain.Files.Events.StoredFiles
{

	public sealed class StoredFileMetadataUpdatedEvent : IDomainEvent
	{
		public StoredFileMetadataUpdatedEvent(StoredFile file)
		{
			File = file;
			OccurredOn = DateTime.UtcNow;
		}

		public StoredFile File { get; }
		public DateTime OccurredOn { get; }
	}
}

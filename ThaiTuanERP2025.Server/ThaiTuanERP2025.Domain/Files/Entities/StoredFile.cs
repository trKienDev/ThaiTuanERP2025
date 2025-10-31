using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Files.Events.StoredFiles;

namespace ThaiTuanERP2025.Domain.Files.Entities
{
	public class StoredFile : AuditableEntity
	{
		public string Bucket { get; private set; } = null!;
		public string ObjectKey { get; private set; } = null!;
		public string FileName { get; private set; } = null!;
		public string ContentType { get; private set; } = null!;
		public long Size { get; private set; }
		public string? Hash { get; private set; }

		public string Module { get; private set; } = null!;
		public string Entity { get; private set; } = null!;
		public string? EntityId { get; private set; }

		public bool IsPublic { get; private set; }

		private StoredFile() { } // EF only

		public StoredFile(
		    string bucket, string objectKey, string fileName, string contentType, long size, string module,
		    string entity, string? entityId = null, string? hash = null, bool isPublic = false
		) {
			Guard.AgainstNullOrWhiteSpace(bucket, nameof(bucket));
			Guard.AgainstNullOrWhiteSpace(objectKey, nameof(objectKey));
			Guard.AgainstNullOrWhiteSpace(fileName, nameof(fileName));
			Guard.AgainstNullOrWhiteSpace(contentType, nameof(contentType));
			Guard.AgainstNegative(size, nameof(size));
			Guard.AgainstNullOrWhiteSpace(module, nameof(module));
			Guard.AgainstNullOrWhiteSpace(entity, nameof(entity));

			Id = Guid.NewGuid();

			Bucket = bucket.Trim();
			ObjectKey = objectKey.Trim();
			FileName = fileName.Trim();
			ContentType = contentType.Trim();
			Size = size;
			Module = module.Trim();
			Entity = entity.Trim();
			EntityId = entityId;
			Hash = hash;
			IsPublic = isPublic;

			AddDomainEvent(new StoredFileCreatedEvent(this));
		}

		#region Domain Behaviors

		public void MakePublic()
		{
			if (IsPublic) return;
			IsPublic = true;
			AddDomainEvent(new StoredFileMadePublicEvent(this));
		}

		public void MakePrivate()
		{
			if (!IsPublic) return;
			IsPublic = false;
			AddDomainEvent(new StoredFileMadePrivateEvent(this));
		}

		public void ChangeEntityReference(string module, string entity, string? entityId)
		{
			Guard.AgainstNullOrWhiteSpace(module, nameof(module));
			Guard.AgainstNullOrWhiteSpace(entity, nameof(entity));

			Module = module;
			Entity = entity;
			EntityId = entityId;
			AddDomainEvent(new StoredFileEntityReferenceChangedEvent(this));
		}

		public void UpdateMetadata(string newFileName, string newContentType)
		{
			Guard.AgainstNullOrWhiteSpace(newFileName, nameof(newFileName));
			Guard.AgainstNullOrWhiteSpace(newContentType, nameof(newContentType));

			FileName = newFileName.Trim();
			ContentType = newContentType.Trim();
			AddDomainEvent(new StoredFileMetadataUpdatedEvent(this));
		}

		#endregion
	}
}

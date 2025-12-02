using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.StoredFiles.Services;

namespace ThaiTuanERP2025.Domain.StoredFiles.Entities
{
	public class StoredFile : AuditableEntity
	{
		
		#region Constructors
		private StoredFile() { } 
		public StoredFile(
			string bucket, string objectKey, string fileName, string contentType, long size, string module,
			string entity, string? entityId = null, string? hash = null, bool isPublic = false
		) {
			Guard.AgainstNullOrWhiteSpace(objectKey, nameof(objectKey));
			Guard.AgainstNullOrWhiteSpace(fileName, nameof(fileName));
			Guard.AgainstNullOrWhiteSpace(contentType, nameof(contentType));
			Guard.AgainstNegative(size, nameof(size));
			Guard.AgainstNullOrWhiteSpace(module, nameof(module));
			Guard.AgainstNullOrWhiteSpace(entity, nameof(entity));

                        module = module.Trim();
                        entity = entity.Trim();

                        if (!FileTypeRegistry.IsValidModule(module))
                                throw new DomainException($"Module '{module}' is not registered");

                        if (!FileTypeRegistry.IsValidEntity(module, entity))
                                throw new DomainException($"Entity '{entity}' is not valid for module '{module}'");

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
		}
		#endregion

		#region Properties
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
		#endregion

		#region Domain Behaviors

		internal void MakePublic()
		{
			if (IsPublic) return;
			IsPublic = true;
		}

		internal void MakePrivate()
		{
			if (!IsPublic) return;
			IsPublic = false;
		}

		internal void ChangeEntityReference(string module, string entity, string? entityId)
		{
			Guard.AgainstNullOrWhiteSpace(module, nameof(module));
			Guard.AgainstNullOrWhiteSpace(entity, nameof(entity));

			Module = module;
			Entity = entity;
			EntityId = entityId;
		}

		internal void UpdateMetadata(string newFileName, string newContentType)
		{
			Guard.AgainstNullOrWhiteSpace(newFileName, nameof(newFileName));
			Guard.AgainstNullOrWhiteSpace(newContentType, nameof(newContentType));

			FileName = newFileName.Trim();
			ContentType = newContentType.Trim();
		}

		#endregion
	}
}

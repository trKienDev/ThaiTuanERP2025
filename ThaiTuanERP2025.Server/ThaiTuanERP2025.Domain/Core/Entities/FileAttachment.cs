using ThaiTuanERP2025.Domain.Core.Services;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class FileAttachment : AuditableEntity
	{
		
		#region Constructors
		private FileAttachment() { } 
		public FileAttachment(
			Guid driveObjectId,
			string module,
			string document,
			string? documentId
		) {
			Guard.AgainstNullOrWhiteSpace(module, nameof(module));
			Guard.AgainstNullOrWhiteSpace(document, nameof(document));

                        module = module.Trim();
                        document = document.Trim();

                        if (!FileTypeRegistry.IsValidModule(module))
                                throw new DomainException($"Module '{module}' is not registered");

                        if (!FileTypeRegistry.IsValidEntity(module, document))
                                throw new DomainException($"Entity '{document}' is not valid for module '{document}'");

                        Id = Guid.NewGuid();
			DriveObjectId = driveObjectId;
			Module = module.Trim();
			Document= document.Trim();
			DocumentId = documentId;
		}
		#endregion

		#region Properties
		public Guid DriveObjectId { get; private set; }
		public string Module { get; private set; } = null!;
		public string Document { get; private set; } = null!;
		public string? DocumentId { get; private set; }
		#endregion

		#region Domain Behaviors
		internal void ChangeEntityReference(string module, string document, string? documentId)
		{
			Guard.AgainstNullOrWhiteSpace(module, nameof(module));
			Guard.AgainstNullOrWhiteSpace(document, nameof(document));

			Module = module;
			Document = document;
			DocumentId = documentId;
		}

		#endregion
	}
}

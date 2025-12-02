using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class Follower
	{
		#region Constructors
		private Follower() { } 
		public Follower(Guid documentId, DocumentType documentType, Guid userId)
		{
			Guard.AgainstDefault(userId, nameof(userId));

			DocumentId = documentId;
			DocumentType = documentType;
			UserId = userId;
		}
		#endregion

		#region Properties
		public Guid DocumentId { get; private set; }
		public DocumentType DocumentType { get; private set; }
		public Guid UserId { get; private set; }
		#endregion

		#region Domain Behaviors
		#endregion
	}
}

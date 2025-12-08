using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public sealed class CommentAttachment
	{
		#region EF Constructor
		private CommentAttachment() { }
		public CommentAttachment(Guid commentId, Guid storedFileId)
		{
			Guard.AgainstDefault(commentId, nameof(commentId));
			Guard.AgainstDefault(storedFileId, nameof(storedFileId));

			Id = Guid.NewGuid();
			CommentId = commentId;
			StoredFileId = storedFileId;
		}
		#endregion

		#region Properties
		public Guid Id { get; private set; }
		public Guid CommentId { get; private set; }
		public Comment Comment { get; private set; } = default!;

		public Guid StoredFileId { get; private set; }
		public StoredFile StoredFile { get; private set; } = default!;
		#endregion
	}
}

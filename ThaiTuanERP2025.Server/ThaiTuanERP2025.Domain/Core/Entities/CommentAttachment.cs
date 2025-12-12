using ThaiTuanERP2025.Domain.Shared;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public sealed class CommentAttachment
	{
		#region EF Constructor
		private CommentAttachment() { }
		public CommentAttachment(Guid commentId, Guid fileAttachmentId)
		{
			Guard.AgainstDefault(commentId, nameof(commentId));
			Guard.AgainstDefault(fileAttachmentId, nameof(fileAttachmentId));

			Id = Guid.NewGuid();
			CommentId = commentId;
			FileAttachmentId = fileAttachmentId;
		}
		#endregion

		#region Properties
		public Guid Id { get; private set; }
		public Guid CommentId { get; private set; }
		public Comment Comment { get; private set; } = default!;

		public Guid FileAttachmentId { get; private set; }
		public FileAttachment FileAttachment { get; private set; } = default!;
		#endregion
	}
}

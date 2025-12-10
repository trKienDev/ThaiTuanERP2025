using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public sealed class CommentMention
	{
		#region EF Constructor
		private CommentMention() { }
		public CommentMention(Guid commentId, Guid userId)
		{
			Id = Guid.NewGuid();
			CommentId = commentId;
			UserId = userId;
		}
		#endregion

		#region Properties
		public Guid Id { get; private set; }
		public Guid CommentId { get; private set; }
		public Guid UserId { get; private set; }

		public Comment Comment { get; private set; } = default!;
		public User User { get; private set; } = default!;
		#endregion
	}
}

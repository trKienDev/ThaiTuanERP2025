using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Core.Comments.Contracts
{
	public sealed record CommentDto
	{
		public DocumentType DocumentType { get; init; }
		public Guid DocumentId { get; init; }
		public Guid UserId { get; init; }
		public string Content { get; init; } = string.Empty;
	}

	public sealed record CommentDetailDto
	{
		public Guid Id { get; init; }
                public DocumentType DocumentType { get; init; }
                public Guid DocumentId { get; init; }
                public Guid UserId { get; init; }
		public UserBriefAvatarDto User { get; init; }
		public string Content { get; init; } = string.Empty;
		public DateTime CreatedAt { get; init; }

                public Guid? ParentCommentId { get; set; }
                public List<CommentDetailDto> Replies { get; set; }
        }
}

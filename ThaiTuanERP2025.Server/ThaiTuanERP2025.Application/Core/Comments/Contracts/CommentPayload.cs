using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Core.Comments.Contracts
{
	public sealed record CommentPayload
	{
		public string DocumentType { get; init; }
		public Guid DocumentId { get; init;  }
		public string Content { get; init;  } = string.Empty;
		public IEnumerable<Guid> AttachmentIds { get; init;  } = Array.Empty<Guid>();	
	}
}

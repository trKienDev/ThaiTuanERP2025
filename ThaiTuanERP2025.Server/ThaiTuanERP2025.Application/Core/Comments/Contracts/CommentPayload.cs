namespace ThaiTuanERP2025.Application.Core.Comments.Contracts
{
	public sealed record CommentPayload
	{
		public string Module { get; init; } = string.Empty;
		public string Entity { get; init; } = string.Empty;
		public Guid EntityId { get; init;  }
		public string Content { get; init;  } = string.Empty;
	}
}

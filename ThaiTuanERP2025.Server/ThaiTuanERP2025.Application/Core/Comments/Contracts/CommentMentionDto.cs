namespace ThaiTuanERP2025.Application.Core.Comments.Contracts
{
	public sealed record CommentMentionDto
	{
		public Guid UserId { get; init; }
		public string FullName { get; init; } = string.Empty;
	}
}

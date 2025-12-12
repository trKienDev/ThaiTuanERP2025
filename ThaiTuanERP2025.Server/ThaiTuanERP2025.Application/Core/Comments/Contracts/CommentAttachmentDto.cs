namespace ThaiTuanERP2025.Application.Core.Comments.Contracts
{
	public sealed record CommentAttachmentDto
	{
		public Guid Id { get; init; }
		public Guid CommentId { get; init; }
		public Guid StoredFileId { get; init; }
		//public StoredFileMetadataDto StoredFile { get; set; }
	}
}

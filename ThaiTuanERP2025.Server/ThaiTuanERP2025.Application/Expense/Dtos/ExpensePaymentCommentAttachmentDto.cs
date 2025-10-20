namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ExpensePaymentCommentAttachmentDto(
		Guid Id,
		string FileName,
		string FileUrl,
		long FileSize,
		string? MimeType,
		Guid FileId
	);
	public sealed record CommentAttachmentRequest(
		string FileName,
		string FileUrl,
		long FileSize,
		string? MimeType,
		Guid? FileId
	);
}

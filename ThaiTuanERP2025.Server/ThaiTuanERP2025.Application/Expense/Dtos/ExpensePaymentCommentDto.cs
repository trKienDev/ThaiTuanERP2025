namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ExpensePaymentCommentDto(
		Guid Id,
		Guid ExpensePaymentId,
		Guid? ParentCommentId,
		string Content,
		bool IsEdited,
		int CommentType,            // ExpensePaymentCommentType -> int
		Guid CreatedByUserId,
		string CreatedByFullName,   // để UI show tên
		string? CreatedByAvatar,    // để UI show avatar
		DateTime CreatedDate,
		IReadOnlyList<ExpensePaymentCommentAttachmentDto> Attachments,
		IReadOnlyList<ExpensePaymentCommentTagDto> Tags,
		List<ExpensePaymentCommentDto> Replies
	) {
		public ExpensePaymentCommentDto() : this(
			default, default, null, "", false, 0, default,  "",  null,  default, 
			new List<ExpensePaymentCommentAttachmentDto>(), 
			new List<ExpensePaymentCommentTagDto>(), 
			new List<ExpensePaymentCommentDto>()
		) { }
	}

	// DTO dùng cho AddComment (request từ FE)
	public sealed record ExpensePaymentCommentRequest(
		Guid ExpensePaymentId,
		Guid? ParentCommentId,
		string Content,
		IReadOnlyList<Guid>? TaggedUserIds,
		IReadOnlyList<CommentAttachmentRequest>? Attachments
	);
}

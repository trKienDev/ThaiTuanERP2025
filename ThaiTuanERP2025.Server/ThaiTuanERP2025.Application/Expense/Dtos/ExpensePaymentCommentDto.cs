using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ExpensePaymentCommentDto
	{
		public Guid Id { get; init; }
		public Guid ExpensePaymentId { get; init; }
		public Guid? ParentCommentId { get; init; }
		public string Content { get; init; } = string.Empty;
		public bool IsEdited { get; init; }
		public int CommentType { get; init; }                  // ExpensePaymentCommentType -> int
		public Guid CreatedByUserId { get; init; }
		public UserDto CreatedByUser { get; init; } = default!;         // cho phép null nếu chưa map
		public DateTime CreatedDate { get; init; }

		public IReadOnlyList<ExpensePaymentCommentAttachmentDto> Attachments { get; init; }
		    = Array.Empty<ExpensePaymentCommentAttachmentDto>();
		public IReadOnlyList<ExpensePaymentCommentTagDto> Tags { get; init; }
		    = Array.Empty<ExpensePaymentCommentTagDto>();

		// Dùng List để có thể .Add() con mà không đụng init-only setter
		public List<ExpensePaymentCommentDto> Replies { get; init; } = new();
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

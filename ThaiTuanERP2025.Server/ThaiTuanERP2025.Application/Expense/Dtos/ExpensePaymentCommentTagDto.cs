namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ExpensePaymentCommentTagDto(
		Guid UserId,
		string? FullName,       // optional: để UI show tên
		string? AvatarObjectKey // optional: để UI show avatar
	);
}

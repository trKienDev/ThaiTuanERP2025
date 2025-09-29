namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ExpensePaymentFollowersDto
	{
		public Guid Id { get; init; }
		public Guid ExpensePaymentId { get; init; }
		public Guid UserId { get; init; }
	}
	public sealed record ExpensePaymentFollowersRequest
	{
		public Guid ExpensePaymentId { get; init; }
		public Guid UserId { get; init; }
	}
}

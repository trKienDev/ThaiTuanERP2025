namespace ThaiTuanERP2025.Application.Expense.Contracts.Resolvers
{
	public sealed record ResolverContext(
		Guid CreatorId,
		decimal Amount,
		string? BudgetCode,
		string? CostCenter,
		string DocumentType
	);
}

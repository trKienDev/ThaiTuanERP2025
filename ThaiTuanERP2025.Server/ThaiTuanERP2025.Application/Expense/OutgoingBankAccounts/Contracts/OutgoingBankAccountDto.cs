namespace ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Contracts
{
	public sealed record OutgoingBankAccountDto
	{
		public string Name { get; init; } = null!;
		public string BankName { get; init; } = null!;
		public string AccountNumber { get; init; } = null!;
		public string OwnerName { get; init; } = null!;
		public bool IsActive { get; init; }
	}
}

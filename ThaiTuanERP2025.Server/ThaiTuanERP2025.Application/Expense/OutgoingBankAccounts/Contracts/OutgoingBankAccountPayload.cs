namespace ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Contracts
{
	public sealed record OutgoingBankAccountPayload
	{
		public string Name { get; init; } = null!;
		public string BankName { get; init; } = null!;
		public string AccountNumber { get; init; } = null!;
		public string OwnerName { get; init; } = null!;
	}
}

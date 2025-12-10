namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts
{
	public sealed record OutgoingPaymentPayload
	{
		public string Name { get; init; } = string.Empty;
		public decimal OutgoingAmount { get; init; }	
		public string BankName { get; init; } = string.Empty;
		public string AccountNumber { get; init; } = string.Empty;	
		public string BeneficiaryName { get; init; } = string.Empty;	
		public DateTime DueAt { get; init; } 
		public Guid OutgoingBankAccountId { get; init; }
		public Guid ExpensePaymentId { get; init; }
		public string? Description { get; init; }
	}
}

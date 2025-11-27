namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts
{
	public sealed record ExpensePaymentDto
	{
		public Guid Id { get; set; } = Guid.Empty;
		public string Name { get; set;} = string.Empty;	 
	}
}

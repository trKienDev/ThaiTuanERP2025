namespace ThaiTuanERP2025.Domain.Expense.Enums
{
	public enum ExpenseWorkflowStatus : byte
	{
		Draft = 0,
		InProgress = 1,
		Approved = 2,
		Rejected = 3,
		Cancelled = 4,
		Expired = 5	
	}
}

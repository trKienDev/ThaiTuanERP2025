namespace ThaiTuanERP2025.Domain.Expense.Enums
{
	public enum StepStatus : byte
	{
		Pending = 0, // Chưa kích hoạt
		Waiting = 1, // chờ duyệt
		Approved = 2,
		Rejected = 3,
		Skipped = 4,
		Expired = 5
	}
}

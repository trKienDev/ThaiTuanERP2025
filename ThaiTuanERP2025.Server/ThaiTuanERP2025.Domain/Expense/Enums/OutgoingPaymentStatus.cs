namespace ThaiTuanERP2025.Domain.Expense.Enums
{
	public enum OutgoingPaymentStatus
	{
		Pending = 0,        // chờ tạo lệnh
		Approved = 1,  // Đã duyệt
		Created = 2,        // đã tạo lệnh
		Cancelled = 3, // Hủy
	}
}

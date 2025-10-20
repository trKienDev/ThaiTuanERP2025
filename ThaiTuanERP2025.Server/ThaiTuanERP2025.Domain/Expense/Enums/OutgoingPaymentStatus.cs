namespace ThaiTuanERP2025.Domain.Expense.Enums
{
	public enum OutgoingPaymentStatus
	{
		Pending = 0,        // chờ tạo lệnh
		Approved = 1,  // Đã duyệt
		Created = 2,        // đã tạo lệnh
		Recorded = 3,        // đã ghi nhận
		Cancelled = 4, // Hủy
	}
}

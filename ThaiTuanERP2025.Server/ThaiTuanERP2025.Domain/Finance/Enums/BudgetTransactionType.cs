namespace ThaiTuanERP2025.Domain.Finance.Enums
{
	public enum BudgetTransactionType
	{
		/// Ghi giảm ngân sách do phát sinh thanh toán thực tế
		ExpensePayment = 1,

		/// Ghi giảm ngân sách do phát sinh tạm ứng thực tế
		AdvancedPayment = 2,

		/// Ghi giảm ngân sách do phát sinh thanh toán tạm ứng thực tế
		AdvancedExpensePayment = 3,

		/// Ghi tăng ngân sách do hoàn lại hoặc hủy chi.
		Refund = 4,

		/// Giữ chỗ ngân sách (tạm khóa khi tạo yêu cầu chi, chưa duyệt).
		Reserve = 5,

		/// Điều chỉnh thủ công bởi người quản trị (ví dụ cập nhật số dư).
		Adjust = 6,
	}
}

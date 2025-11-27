namespace ThaiTuanERP2025.Domain.Finance.Enums
{
	public enum BudgetTransactionType
	{
		Debit = 1,       // Chi tiền (tạo Payment)
		Credit = 2,      // Hoàn ngân sách (hủy Payment)
		Adjustment = 3,  // Điều chỉnh (Payment sửa số tiền)
		Reverse = 4
	}
}

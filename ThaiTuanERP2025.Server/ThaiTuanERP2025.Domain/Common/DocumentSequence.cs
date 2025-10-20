namespace ThaiTuanERP2025.Domain.Common
{
	/// <summary>
	/// Dùng để lưu số thứ tự (sequence) theo key bất kỳ, ví dụ:
	/// Key = "ExpensePayment:20251006" => LastNumber = 123
	/// </summary>
	public class DocumentSequence
	{
		public string Key { get; set; } = default!;  // ví dụ: "ExpensePayment:20250610"
		public long LastNumber { get; set; }
	}
}

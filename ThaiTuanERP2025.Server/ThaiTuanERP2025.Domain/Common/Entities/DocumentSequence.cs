namespace ThaiTuanERP2025.Domain.Common.Entities
{
	/// <summary>
	/// Dùng để lưu số thứ tự (sequence) theo key bất kỳ, ví dụ:
	/// Key = "ExpensePayment:20251006" => LastNumber = 123
	/// </summary>
	public class DocumentSequence : AuditableEntity
	{
		private DocumentSequence() { } // EF cần

		public string Key { get; private set; } = default!;
		public long LastNumber { get; private set; }

		public DocumentSequence(string key, long initialNumber = 0)
		{
			Guard.AgainstNullOrWhiteSpace(key, nameof(key));
			Guard.AgainstNegative(initialNumber, nameof(initialNumber));

			Key = key.Trim();
			LastNumber = initialNumber;
		}

		/// <summary>
		/// Tăng sequence và trả về giá trị mới (đồng thời đảm bảo không âm)
		/// </summary>
		public long Next()
		{
			LastNumber++;
			return LastNumber;
		}
	}
}

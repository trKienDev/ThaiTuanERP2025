using System.ComponentModel.DataAnnotations;

namespace ThaiTuanERP2025.Domain.Common.Entities
{
	public class NumberSeries : BaseEntity
	{
		private NumberSeries() { } // EF

		public string Key { get; private set; } = string.Empty;     // ví dụ: "ExpensePayment"
		public string Prefix { get; private set; } = string.Empty;  // ví dụ: "EP"
		public int PadLength { get; private set; } = 6;
		public long NextNumber { get; private set; } = 1;

		/// <summary>
		/// Dùng cho kiểm soát song song (Optimistic Concurrency)
		/// </summary>
		[Timestamp]
		public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

		public NumberSeries(string key, string prefix, int padLength = 6, long startNumber = 1)
		{
			Guard.AgainstNullOrWhiteSpace(key, nameof(key));
			Guard.AgainstNullOrWhiteSpace(prefix, nameof(prefix));
			Guard.AgainstNegativeOrZero(padLength, nameof(padLength));
			Guard.AgainstNegative(startNumber, nameof(startNumber));

			Key = key.Trim();
			Prefix = prefix.Trim();
			PadLength = padLength;
			NextNumber = startNumber;
		}

		/// <summary>
		/// Lấy số tiếp theo và tăng giá trị trong bộ đếm.
		/// </summary>
		public string GenerateNext()
		{
			var formatted = $"{Prefix}{NextNumber.ToString().PadLeft(PadLength, '0')}";
			NextNumber++;
			return formatted;
		}
	}
}

using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Common
{
	/// <summary>
	/// Guard utility giúp bảo vệ invariant của domain entity và value object.
	/// Cung cấp các phương thức xác thực đầu vào để đảm bảo trạng thái hợp lệ.
	/// </summary>
	public static class Guard
	{
		// === Null, Empty, and Whitespace Checks ===

		public static void AgainstNull(object? value, string paramName)
		{
			if (value is null)
				throw new DomainException($"{paramName} không được null");
		}

		public static void AgainstNullOrWhiteSpace(string? value, string paramName)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new DomainException($"{paramName} không được để trống");
		}

		public static void AgainstNullOrEmptyGuid(Guid value, string paramName)
		{
			if (value == Guid.Empty)
				throw new DomainException($"{paramName} không được là Guid rỗng");
		}

		// === Numeric Checks ===

		public static void AgainstNegative(decimal value, string paramName)
		{
			if (value < 0)
				throw new DomainException($"{paramName} không được âm");
		}

		public static void AgainstZeroOrNegative(decimal value, string paramName)
		{
			if (value <= 0)
				throw new DomainException($"{paramName} phải lớn hơn 0");
		}

		public static void AgainstOutOfRange(decimal value, decimal min, decimal max, string paramName)
		{
			if (value < min || value > max)
				throw new DomainException($"{paramName} phải nằm trong khoảng [{min}, {max}]");
		}

		// === String Length and Format Checks ===

		public static void AgainstExceedLength(string value, int maxLength, string paramName)
		{
			if (value.Length > maxLength)
				throw new DomainException($"{paramName} vượt quá {maxLength} ký tự");
		}

		public static void AgainstInvalidEmail(string email, string paramName)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new DomainException($"{paramName} không được để trống");

			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				if (addr.Address != email)
					throw new DomainException($"{paramName} không hợp lệ");
			}
			catch
			{
				throw new DomainException($"{paramName} không hợp lệ");
			}
		}

		public static void AgainstInvalidDateRange(DateTime startDate, DateTime endDate, string paramName)
		{
			if (startDate > endDate)
				throw new DomainException($"{paramName}: Ngày bắt đầu không được lớn hơn ngày kết thúc");
		}

		// === Enum Validation ===

		public static void AgainstInvalidEnumValue<TEnum>(TEnum value, string paramName) where TEnum : struct, Enum
		{
			if (!Enum.IsDefined(typeof(TEnum), value))
				throw new DomainException($"{paramName} có giá trị không hợp lệ: {value}");
		}
	}
}

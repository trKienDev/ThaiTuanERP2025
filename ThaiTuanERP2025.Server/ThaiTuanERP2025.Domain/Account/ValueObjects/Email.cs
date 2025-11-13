using System.Text.RegularExpressions;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.ValueObjects;

namespace ThaiTuanERP2025.Domain.Account.ValueObjects
{
	public sealed class Email : ValueObject
	{
		public string Value { get; init; } = string.Empty;

		private static readonly Regex EmailRegex = new(
			@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
			RegexOptions.Compiled | RegexOptions.IgnoreCase
		);

		private Email() { } // EF Core only

		public Email(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Email không được để trống.", nameof(value));
			if (!EmailRegex.IsMatch(value))
				throw new ArgumentException("Email không đúng định dạng.", nameof(value));

			Guard.AgainstInvalidEmail(value, nameof(value));

			Value = value;
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value.ToLowerInvariant(); // So sánh không phân biệt hoa thường
		}

		public override string ToString() => Value;
	}
}
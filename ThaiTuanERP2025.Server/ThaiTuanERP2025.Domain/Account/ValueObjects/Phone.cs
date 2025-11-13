using System.Text.RegularExpressions;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.ValueObjects;


namespace ThaiTuanERP2025.Domain.Account.ValueObjects
{
	public sealed class Phone : ValueObject
	{
		public string Value { get; init; } = string.Empty;

		private static readonly Regex PhoneRegex = new(
			@"^(0|\+84)(\d{9})$",
			RegexOptions.Compiled
		);

		private Phone() { } // EF Core only

		public Phone(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(nameof(value), "Số điện thoại không được để trống.");
			if (!PhoneRegex.IsMatch(value))
				throw new ArgumentException("Số điện thoại không đúng định dạng.", nameof(value));

			Guard.AgainstInvalidPhone(value, nameof(value));
			Value = value;
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value;
		}

		public override string ToString() => Value;
	}
}

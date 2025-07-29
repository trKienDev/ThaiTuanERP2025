using System;
using System.Text.RegularExpressions;

namespace ThaiTuanERP2025.Domain.Common
{
	public sealed class Email : ValueObject
	{
		public string Value { get; }

		private static readonly Regex EmailRegex = new(
			@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
			RegexOptions.Compiled | RegexOptions.IgnoreCase
		);
		
		public Email(string value ) {
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Email không được để trống.", nameof(value));
			if(!EmailRegex.IsMatch(value)) {
				throw new ArgumentException("Email không đúng định dạng.", nameof(value));
			}

			Value = value;
		}

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Value.ToLowerInvariant(); // So sánh không phân biệt hoa thường
		}

		public override string ToString() => Value;	
	}
}
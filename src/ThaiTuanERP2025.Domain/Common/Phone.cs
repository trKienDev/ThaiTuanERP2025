using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Common
{
	public sealed class Phone : ValueObject
	{
		public string Value { get; }

		private static readonly Regex PhoneRegex = new(
			@"^(0|\+84)(\d{9})$",
			RegexOptions.Compiled
		);

		public Phone(string value)
		{
			if(string.IsNullOrWhiteSpace(value)) {
				throw new ArgumentNullException("Số điện thoại không được để trống.", nameof(value));	
			}
			if (!PhoneRegex.IsMatch(value))
			{
				throw new ArgumentException("Số điện thoại không đúng định dạng.", nameof(value));
			}

			Value = value;
		}	

		protected override IEnumerable<object> GetEqualityComponents() {
			yield return Value;
		}

		public override string ToString() => Value;
	}
}

using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.ValueObjects
{
	public sealed class RoleName : IEquatable<RoleName>
	{
		public string Value { get; }
		private RoleName(string value) => Value = value;

		public static RoleName Create(string value)
		{
			Guard.AgainstNullOrWhiteSpace(value, nameof(value));
			Guard.AgainstExceedLength(value, 100, nameof(value));

			return new RoleName(value.Trim());
		}

		public override string ToString() => Value;

		public bool Equals(RoleName? other) => other is not null && Value == other.Value;
		public override bool Equals(object? obj) => Equals(obj as RoleName);
		public override int GetHashCode() => Value.GetHashCode();
	}
}

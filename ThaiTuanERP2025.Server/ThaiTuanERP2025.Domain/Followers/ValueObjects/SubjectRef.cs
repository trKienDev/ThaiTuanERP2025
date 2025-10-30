using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Followers.Enums;

namespace ThaiTuanERP2025.Domain.Followers.ValueObjects
{
	public readonly record struct SubjectRef
	{
		public SubjectType Type { get; }
		public Guid Id { get; }

		public SubjectRef(SubjectType type, Guid id)
		{
			Guard.AgainstInvalidEnumValue(type, nameof(type));
			Guard.AgainstDefault(id, nameof(id));

			Type = type;
			Id = id;
		}

		public override string ToString() => $"{Type}:{Id}";
	}
}

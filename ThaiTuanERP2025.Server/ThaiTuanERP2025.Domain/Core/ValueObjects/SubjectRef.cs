using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Domain.Followers.ValueObjects
{
	public record class SubjectRef
	{
		public SubjectType Type { get; init; }
		public Guid Id { get; init; }

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

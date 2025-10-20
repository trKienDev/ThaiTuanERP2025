using ThaiTuanERP2025.Domain.Followers.Enums;

namespace ThaiTuanERP2025.Domain.Followers.Entities
{
	public sealed class SubjectRef
	{
		private SubjectRef() { }
		public SubjectRef(SubjectType type, Guid id)
		{
			Type = type;
			Id = id;
		}

		public SubjectType Type { get; private set; } = default!;
		public Guid Id { get; private set; }
	}
}

namespace ThaiTuanERP2025.Domain.Common
{
	public abstract class BaseEntity
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public override bool Equals(object? obj)
		{
			if (obj is not BaseEntity other) return false;
			if(ReferenceEquals(this, other)) return true;

			return Id == other.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}

using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Common
{
	public abstract class BaseEntity
	{
		public Guid Id { get; protected set; } = Guid.NewGuid();

		// --- Domain Events ---
		private readonly List<IDomainEvent> _domainEvents = new();
		public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

		protected void AddDomainEvent(IDomainEvent @event)
		{
			_domainEvents.Add(@event);
		}

		public void ClearDomainEvents() => _domainEvents.Clear();

		// === Equality override ===
		public override bool Equals(object? obj)
		{
			if (obj is not BaseEntity other) return false;
			if (ReferenceEquals(this, other)) return true;
			return Id == other.Id;
		}

		public override int GetHashCode() => Id.GetHashCode();
	}
}

using ThaiTuanERP2025.Domain.Shared.Events;

namespace ThaiTuanERP2025.Application.Shared.Events
{
	public interface IDomainEventDispatcher
	{
		Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
	}
}

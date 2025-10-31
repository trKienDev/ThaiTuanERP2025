using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Application.Common.Events
{
	public interface IDomainEventDispatcher
	{
		Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
	}
}

using MediatR;
using ThaiTuanERP2025.Application.Common.Events;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Infrastructure.Common
{
	/// <summary>
	/// Dịch vụ trung gian dùng để publish các domain event sau khi SaveChangesAsync().
	/// </summary>
	public sealed class DomainEventDispatcher : IDomainEventDispatcher
	{
		private readonly IMediator _mediator;

		public DomainEventDispatcher(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
		{
			Console.WriteLine($"[Dispatcher] Publishing event: {domainEvents.GetType().Name}");
			foreach (var domainEvent in domainEvents)
			{
				await _mediator.Publish(domainEvent, cancellationToken);
			}
		}
	}
}

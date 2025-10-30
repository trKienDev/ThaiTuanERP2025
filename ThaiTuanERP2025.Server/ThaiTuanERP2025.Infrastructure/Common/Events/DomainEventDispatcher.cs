using MediatR;
using ThaiTuanERP2025.Domain.Common;
namespace ThaiTuanERP2025.Infrastructure.Common
{
	/// <summary>
	/// Dịch vụ trung gian dùng để publish các domain event sau khi SaveChangesAsync().
	/// </summary>
	public sealed class DomainEventDispatcher
	{
		private readonly IMediator _mediator;

		public DomainEventDispatcher(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task DispatchAsync(IEnumerable<AuditableEntity> entitiesWithEvents, CancellationToken cancellationToken = default)
		{
			foreach (var entity in entitiesWithEvents)
			{
				var domainEvents = entity.DomainEvents.ToList();
				entity.ClearDomainEvents();

				foreach (var domainEvent in domainEvents)
				{
					await _mediator.Publish(domainEvent, cancellationToken);
				}
			}
		}
	}
}

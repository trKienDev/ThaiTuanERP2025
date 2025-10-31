using MediatR;
using ThaiTuanERP2025.Application.Common.Events;

namespace ThaiTuanERP2025.Infrastructure.Common.Events
{
	/// <summary>
	/// Implementation chuẩn cho IApplicationEventPublisher —  sử dụng MediatR để publish Application Events.
	/// </summary>
	public sealed class ApplicationEventPublisher : IApplicationEventPublisher
	{
		private readonly IPublisher _publisher;

		public ApplicationEventPublisher(IPublisher publisher)
		{
			_publisher = publisher;
		}

		public async Task PublishAsync(IApplicationEvent @event, CancellationToken cancellationToken = default)
		{
			if (@event is null)
				throw new ArgumentNullException(nameof(@event));

			await _publisher.Publish(@event, cancellationToken);
		}
	}
}

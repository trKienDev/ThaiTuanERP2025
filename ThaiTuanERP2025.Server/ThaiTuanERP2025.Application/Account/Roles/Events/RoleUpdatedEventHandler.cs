using MediatR;
using ThaiTuanERP2025.Domain.Account.Events;

namespace ThaiTuanERP2025.Application.Account.Roles.Events
{
	public sealed class RoleUpdatedEventHandler : INotificationHandler<RoleUpdatedEvent>
	{
		public Task Handle(RoleUpdatedEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[DomainEvent] ✏️ Role updated: {notification.Role.Name}");
			return Task.CompletedTask;
		}
	}
}

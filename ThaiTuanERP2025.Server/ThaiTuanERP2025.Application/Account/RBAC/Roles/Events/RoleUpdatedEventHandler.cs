using MediatR;
using ThaiTuanERP2025.Domain.Account.Events.Roles;

namespace ThaiTuanERP2025.Application.Account.EventHandlers
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

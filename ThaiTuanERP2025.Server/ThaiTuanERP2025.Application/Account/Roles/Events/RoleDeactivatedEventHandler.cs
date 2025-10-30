using MediatR;
using ThaiTuanERP2025.Domain.Account.Events.Roles;

namespace ThaiTuanERP2025.Application.Account.Roles.Events
{
	public sealed class RoleDeactivatedEventHandler : INotificationHandler<RoleDeactivatedEvent>
	{
		public Task Handle(RoleDeactivatedEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[DomainEvent] 🚫 Role deactivated: {notification.Role.Name}");
			return Task.CompletedTask;
		}
	}
}

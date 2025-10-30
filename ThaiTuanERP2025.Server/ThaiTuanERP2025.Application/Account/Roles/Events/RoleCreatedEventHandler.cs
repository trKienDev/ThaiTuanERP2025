using MediatR;
using ThaiTuanERP2025.Domain.Account.Events.Roles;

namespace ThaiTuanERP2025.Application.Account.Roles.Events
{
	public sealed class RoleCreatedEventHandler : INotificationHandler<RoleCreatedEvent>
	{
		public Task Handle(RoleCreatedEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[DomainEvent] 🟢 Role created: {notification.Role.Name} (Id: {notification.Role.Id})");
			return Task.CompletedTask;
		}
	}
}

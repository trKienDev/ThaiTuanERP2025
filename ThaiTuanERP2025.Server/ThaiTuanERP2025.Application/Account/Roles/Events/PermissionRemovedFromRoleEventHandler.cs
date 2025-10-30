using MediatR;
using ThaiTuanERP2025.Domain.Account.Events.Roles;

namespace ThaiTuanERP2025.Application.Account.Roles.Events
{
	public sealed class PermissionRemovedFromRoleEventHandler : INotificationHandler<PermissionRemovedFromRoleEvent>
	{
		public Task Handle(PermissionRemovedFromRoleEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[DomainEvent] 🗑️ Permission {notification.PermissionId} removed from Role {notification.Role.Name}");
			return Task.CompletedTask;
		}
	}
}

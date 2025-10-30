using MediatR;
using ThaiTuanERP2025.Domain.Account.Events.Roles;

namespace ThaiTuanERP2025.Application.Account.RBAC.Roles.Events
{
	public sealed class UserRemovedFromRoleEventHandler : INotificationHandler<UserRemovedFromRoleEvent>
	{
		public Task Handle(UserRemovedFromRoleEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[DomainEvent] ❌ User {notification.UserId} removed from Role {notification.Role.Name}");
			return Task.CompletedTask;
		}
	}
}

using MediatR;
using ThaiTuanERP2025.Domain.Account.Events;

namespace ThaiTuanERP2025.Application.Account.Roles.Events
{
	public sealed class PermissionAssignedToRoleEventHandler : INotificationHandler<PermissionAssignedToRoleEvent>
	{
		public Task Handle(PermissionAssignedToRoleEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[DomainEvent] 🔐 Permission {notification.PermissionId} assigned to Role {notification.Role.Name}");
			return Task.CompletedTask;
		}
	}
}
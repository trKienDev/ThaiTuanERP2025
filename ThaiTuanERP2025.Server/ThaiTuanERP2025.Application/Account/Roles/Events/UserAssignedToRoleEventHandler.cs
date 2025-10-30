using MediatR;
using ThaiTuanERP2025.Domain.Account.Events.Roles;

namespace ThaiTuanERP2025.Application.Account.Roles.Events
{
	public sealed class UserAssignedToRoleEventHandler : INotificationHandler<UserAssignedToRoleEvent>
	{
		public Task Handle(UserAssignedToRoleEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[DomainEvent] 👤 User {notification.UserId} assigned to Role {notification.Role.Name}");
			return Task.CompletedTask;
		}
	}
}

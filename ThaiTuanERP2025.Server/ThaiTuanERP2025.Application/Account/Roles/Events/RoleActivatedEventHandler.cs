using MediatR;
using ThaiTuanERP2025.Domain.Account.Events;

namespace ThaiTuanERP2025.Application.Account.Roles.Events
{
	public sealed class RoleActivatedEventHandler : INotificationHandler<RoleActivatedEvent>
	{
		public Task Handle(RoleActivatedEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"[DomainEvent] ✅ Role activated: {notification.Role.Name}");
			return Task.CompletedTask;
		}
	}
}

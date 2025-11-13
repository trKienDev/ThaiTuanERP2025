using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Core.Services
{
	
	public class ReminderService : IReminderService
	{
		private readonly IUnitOfWork _uow;
		private readonly IRealtimeNotifier _realtime;
		public ReminderService(IUnitOfWork uow, IRealtimeNotifier realtime) {
			_uow = uow;
			_realtime = realtime;
		}

		public async Task ScheduleReminderAsync(Guid userId, string subject, string message, int slaHours, DateTime dueAt, string? linkUrl = null, CancellationToken cancellationToken = default)
		{
			var reminder = new UserReminder(userId, subject, message, slaHours, dueAt, linkUrl);
			await _uow.UserReminders.AddAsync(reminder, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);

			var payload = new UserReminderDto {
				Id = reminder.Id,
				Subject = reminder.Subject,
				Message = reminder.Message,
				SlaHours = reminder.SlaHours,
				DueAt = reminder.DueAt,
				IsResolved = reminder.IsResolved,
				ResolvedAt = reminder.ResolvedAt,
				LinkUrl = reminder.LinkUrl
			};
			await _realtime.PushRemindersAsync(new[] { userId }, new[] { payload }, cancellationToken);
		}
	}
}

using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Services
{
	public interface IReminderService
	{
		Task ScheduleReminderAsync(Guid userId, string subject, string message, DateTime triggerAt, string? linkUrl = null, CancellationToken cancellationToken = default);
	}

	public class ReminderService : IReminderService
	{
		private readonly IUnitOfWork _uow;
		public ReminderService(IUnitOfWork uow) {
			_uow = uow;
		}

		public async Task ScheduleReminderAsync(Guid userId, string subject, string message, DateTime triggerAt, string? linkUrl = null, CancellationToken cancellationToken = default)
		{
			var reminder = new UserReminder(userId, subject, message, triggerAt, linkUrl);
			await _uow.UserReminders.AddAsync(reminder, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}
	}
}

using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Application.Core.Reminders;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Application.Core.Reminders.Contracts;
using System.Text.Json;
using ThaiTuanERP2025.Application.Shared.Exceptions;

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

		public async Task ScheduleReminderAsync (
			Guid userId, string subject, string message, int slaHours, DateTime dueAt,
			LinkType linkType, Guid targetId,
			CancellationToken cancellationToken = default
		) {
			// === Reminder ===
			var reminder = new UserReminder(userId, subject, message, slaHours, dueAt, linkType, targetId);
			await _uow.UserReminders.AddAsync(reminder, cancellationToken);

			// === Outbox ===
			var outboxMessagePayload = new ReminderCreatedPayload (
				userId,
				reminder.Id,
				reminder.Subject,
				reminder.Message,
				reminder.LinkUrl,
				reminder.SlaHours,
				reminder.DueAt
			);
			var json = JsonSerializer.Serialize(outboxMessagePayload);
			var outbox = new OutboxMessage("ReminderCreated", json);
			await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}

		public async Task MarkResolvedAsync(Guid reminderId, CancellationToken cancellationToken = default)
		{
			// 1. Tìm reminder
			var reminder = await _uow.UserReminders.GetByIdAsync(reminderId, cancellationToken);
			if (reminder == null)
				throw new NotFoundException($"Reminder {reminderId} not found");

			// 2. Domain behavior
			reminder.MarkResolved();

			// 3. Ghi Outbox để push RealTime / Notification
			var payload = new ReminderResolvedPayload(
				reminder.Id,
				reminder.UserId,
				reminder.Subject,
				reminder.ResolvedAt!.Value
			);

			var json = JsonSerializer.Serialize(payload);
			var outbox = new OutboxMessage("ReminderResolved", json);
			await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);

			// 4. Lưu
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}
	}
}

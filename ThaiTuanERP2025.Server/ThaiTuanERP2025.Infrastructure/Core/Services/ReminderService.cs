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
		public ReminderService(IUnitOfWork uow) {
			_uow = uow;
		}

		#region Schedule
		// ==== PUBLIC ====
		public async Task ScheduleReminderAsync(Guid userId, string subject, string message, int slaHours, DateTime dueAt, LinkType linkType, Guid targetId, CancellationToken cancellationToken = default) {
			var (reminder, outbox) = CreateReminderInternal(userId, subject, message, slaHours, dueAt, linkType, targetId);

			await _uow.UserReminders.AddAsync(reminder, cancellationToken);
			await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}

		public async Task ScheduleReminderManyAsync(IEnumerable<Guid> userIds, string subject, string message, int slaHours, DateTime dueAt, LinkType linkType, Guid targetId, CancellationToken cancellationToken = default)
		{
			if(userIds == null || !userIds.Any()) return;

			var reminders = new List<UserReminder>();
			var outboxes = new List<OutboxMessage>();

			foreach (var userId in userIds.Distinct())
			{
				var (reminder, outbox) = CreateReminderInternal(
				    userId, subject, message, slaHours, dueAt, linkType, targetId);

				reminders.Add(reminder);
				outboxes.Add(outbox);
			}

			await _uow.UserReminders.AddRangeAsync(reminders, cancellationToken);
			await _uow.OutboxMessages.AddRangeAsync(outboxes, cancellationToken);

			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}

		// ==== PRIVATE ====
		private (UserReminder reminder, OutboxMessage outbox) CreateReminderInternal(Guid userId, string subject, string message, int slaHours, DateTime dueAt, LinkType linkType, Guid targetId)
		{
			// Create reminder (domain entity)
			var reminder = new UserReminder(userId, subject, message, slaHours, dueAt, linkType, targetId);

			// Create outbox payload
			var payload = new ReminderCreatedPayload(
				userId,
				reminder.Id,
				reminder.Subject,
				reminder.Message,
				reminder.LinkUrl,
				reminder.SlaHours,
				reminder.DueAt
			);

			var json = JsonSerializer.Serialize(payload);
			var outbox = new OutboxMessage("ReminderCreated", json);

			return (reminder, outbox);
		}
		#endregion

		#region Resolve
		// ==== PUBLIC ====
		public async Task MarkResolvedAsync(Guid reminderId, CancellationToken cancellationToken = default)
		{
			var (_, outbox) = await ResolveOneAsync(reminderId, cancellationToken);

			await _uow.OutboxMessages.AddAsync(outbox, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}

		public async Task MarkResolvedManyAsync(IEnumerable<Guid> reminderIds, CancellationToken cancellationToken = default)
		{
			if (reminderIds == null || !reminderIds.Any())
				return;

			var outboxes = new List<OutboxMessage>();

			foreach (var id in reminderIds.Distinct())
			{
				var (_, outbox) = await ResolveOneAsync(id, cancellationToken);
				outboxes.Add(outbox);
			}

			await _uow.OutboxMessages.AddRangeAsync(outboxes, cancellationToken);
			await _uow.SaveChangesWithoutDispatchAsync(cancellationToken);
		}
		
		// ==== Private =====
		private async Task<(UserReminder reminder, OutboxMessage outbox)> ResolveOneAsync(Guid reminderId, CancellationToken cancellationToken)
		{
			var reminder = await _uow.UserReminders.GetByIdAsync(reminderId, cancellationToken);
			if (reminder == null)
				throw new NotFoundException($"Reminder {reminderId} not found");

			reminder.MarkResolved();

			var payload = new ReminderResolvedPayload(
				reminder.Id,
				reminder.UserId,
				reminder.Subject,
				reminder.ResolvedAt!.Value
			);

			var json = JsonSerializer.Serialize(payload);
			var outbox = new OutboxMessage("ReminderResolved", json);

			return (reminder, outbox);
		}
		#endregion
	}
}

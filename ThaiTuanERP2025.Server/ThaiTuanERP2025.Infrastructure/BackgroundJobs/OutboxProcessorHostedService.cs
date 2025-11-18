using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using ThaiTuanERP2025.Application.Core.Notifications.Contracts;
using ThaiTuanERP2025.Application.Core.OutboxMessages;
using ThaiTuanERP2025.Application.Core.Reminders.Contracts;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.BackgroundJobs
{
	public class OutboxProcessorHostedService : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<OutboxProcessorHostedService> _logger;

		private const int BatchSize = 20;
		private const int DelayMilliseconds = 1000;

		public OutboxProcessorHostedService(
			IServiceProvider serviceProvider,
			ILogger<OutboxProcessorHostedService> logger)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("OutboxProcessorHostedService started");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using var scope = _serviceProvider.CreateScope();
					var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
					var outboxRepo = scope.ServiceProvider.GetRequiredService<IOutboxMessageReadRepository>();
					var realtime = scope.ServiceProvider.GetRequiredService<IRealtimeNotifier>();

					var messages = await outboxRepo.GetUnprocessedAsync(BatchSize, stoppingToken);

					if (messages.Count == 0)
					{
						await Task.Delay(DelayMilliseconds, stoppingToken);
						continue;
					}

					foreach (var msg in messages)
					{
						try
						{
							switch (msg.Type)
							{
								case "ReminderCreated":
									await HandleReminderCreatedAsync(msg, realtime, stoppingToken);
									break;

								case "ReminderResolved":
									await HandleReminderResolvedAsync(msg, realtime, stoppingToken);
									break;

								case "NotificationCreated":
									await HandleNotificationCreatedAsync(msg, realtime, stoppingToken);
									break;

								case "NotificationRead":
									await HandleNotificationReadAsync(msg, realtime, stoppingToken);
									break;

								default:
									_logger.LogWarning("Unknown outbox message type: {Type}", msg.Type);
									msg.MarkProcessed();
									break;
							}
						}
						catch (Exception ex)
						{
							_logger.LogError(ex, "Error while processing outbox message {Id}", msg.Id);
							msg.MarkFailed(ex.Message);
						}
					}

					await uow.SaveChangesWithoutDispatchAsync(stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "OutboxProcessorHostedService loop error");
				}

				await Task.Delay(DelayMilliseconds, stoppingToken);
			}

			_logger.LogInformation("OutboxProcessorHostedService stopped");
		}

		private static async Task HandleReminderCreatedAsync(
			OutboxMessage msg,
			IRealtimeNotifier realtime,
			CancellationToken cancellationToken)
		{
			var payload = JsonSerializer.Deserialize<ReminderCreatedPayload>(msg.Payload);
			if (payload is null)
			{
				throw new InvalidOperationException($"Cannot deserialize payload for OutboxMessage {msg.Id}");
			}

			// Build DTO đúng format client đang dùng (UserReminderDto)
			var dto = new UserReminderDto
			{
				Id = payload.ReminderId,
				Subject = payload.Subject,
				Message = payload.Message,
				SlaHours = payload.SlaHours,
				DueAt = payload.DueAt,
				IsResolved = false,
				ResolvedAt = null,
				LinkUrl = payload.LinkUrl
			};

			await realtime.PushRemindersAsync(
				new[] { payload.UserId },
				new[] { dto },
				cancellationToken
			);

			msg.MarkProcessed();
		}

		private static async Task HandleNotificationCreatedAsync(
			OutboxMessage msg,
			IRealtimeNotifier realtime,
			CancellationToken cancellationToken
		) {
			var payload = JsonSerializer.Deserialize<NotificationCreatedPayload>(msg.Payload);
			if (payload is null)
				throw new InvalidOperationException($"Invalid NotificationCreated payload in OutboxMessage {msg.Id}");

			// Build NotificationDto EXACT format Angular expects
			var dto = new UserNotificationDto
			{
				Id = payload.NotificationId,
				SenderId = payload.SenderId,
				Title = payload.Title,
				Message = payload.Message,
				Link = payload.LinkUrl,
				LinkType = payload.LinkType,
				TargetId = payload.TargetId,
				Type = payload.Type,
				CreatedAt = payload.CreatedAt,
				IsRead = payload.IsRead
			};

			// Push to correct user
			await realtime.PushNotificationsAsync(
				new[] { payload.ReceiverId },
				new[] { dto },
				cancellationToken
			);

			msg.MarkProcessed();
		}

		private static async Task HandleReminderResolvedAsync(
			OutboxMessage msg,
			IRealtimeNotifier realtime,
			CancellationToken cancellationToken
		) {
			var payload = JsonSerializer.Deserialize<ReminderResolvedPayload>(msg.Payload);
			if (payload is null)
				throw new InvalidOperationException($"Cannot deserialize payload for OutboxMessage {msg.Id}");

			// Push down exactly what Angular needs: chỉ id để remove khỏi UI
			await realtime.PushReminderResolvedAsync(
				payload.UserId,
				payload.ReminderId,
				cancellationToken
			);

			msg.MarkProcessed();
		}
		private static async Task HandleNotificationReadAsync(
			OutboxMessage msg,
			IRealtimeNotifier realtime,
			CancellationToken cancellationToken
		) {
			var payload = JsonSerializer.Deserialize<NotificationReadPayload>(msg.Payload);
			if (payload is null)
				throw new InvalidOperationException($"Invalid NotificationRead payload in OutboxMessage {msg.Id}");

			// Push realtime về đúng user
			await realtime.PushNotificationReadAsync(
				payload.Id,
				payload.ReceiverId,
				payload.ReadAt,
				cancellationToken
			);

			msg.MarkProcessed();
		}
	}
}

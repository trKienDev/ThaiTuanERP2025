using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
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
			var payload = JsonSerializer.Deserialize<ReminderPayload>(msg.Payload);
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
	}
}

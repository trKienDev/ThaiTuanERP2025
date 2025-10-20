using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Notifications.Services;

namespace ThaiTuanERP2025.Infrastructure.Notifications.Background
{
	public sealed class TaskReminderExpiryHostedService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger<TaskReminderExpiryHostedService> _logger;
		private readonly TaskReminderExpiryOptions _options;
		public TaskReminderExpiryHostedService(
			IServiceScopeFactory scopeFactory,
			ILogger<TaskReminderExpiryHostedService> logger,
			IOptions<TaskReminderExpiryOptions> options)
		{
			_scopeFactory = scopeFactory;
			_logger = logger;
			_options = options.Value;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			_logger.LogInformation(
				"TaskReminderExpiryHostedService started. Interval={Interval}, BatchSize={BatchSize}",
				_options.Interval, 
				_options.BatchSize
			);

			// chạy ngay lần đầu (optional), sau đó theo chu kỳ
			await SweepOnceAsync(stoppingToken);

			using var timer = new PeriodicTimer(_options.Interval);
			while(await timer.WaitForNextTickAsync(stoppingToken))
			{
				await SweepOnceAsync(stoppingToken);
			}
		}

		private async Task SweepOnceAsync(CancellationToken cancellationToken)
		{
			try
			{
				using var scope = _scopeFactory.CreateScope();
				var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
				var notifier = scope.ServiceProvider.GetRequiredService<IRealtimeNotifier>();
				var now = DateTime.UtcNow;

				while (true)
				{
					// Lấy một batch reminders đã quá hạn & chưa resolve
					var batch = await unitOfWork.TaskReminders.ListAsync(q =>
						q.Where(x => !x.IsResolved && x.DueAt <= now)
						 .OrderBy(x => x.DueAt)
						 .Take(_options.BatchSize),
						 cancellationToken: cancellationToken
					);
					if (batch.Count == 0) break; // hết rồi

					// Group theo User để đẩy ResolveAlarm theo nhóm
					var grouped = batch.GroupBy(x => x.UserId);
				}
			}
			catch (OperationCanceledException)
			{
				// bỏ qua
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error sweeping expired task reminders.");
			}
		}
	}
}

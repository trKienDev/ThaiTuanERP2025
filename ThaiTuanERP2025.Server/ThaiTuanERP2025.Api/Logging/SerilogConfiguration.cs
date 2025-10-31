using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;

namespace ThaiTuanERP2025.Api.Logging
{
	public static class SerilogConfiguration
	{
		public static void ConfigureSerilog(WebApplicationBuilder builder)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.Enrich.WithProperty("Application", "ThaiTuanERP2025")
				.Enrich.WithThreadId()
				.Enrich.WithMachineName()
				.WriteTo.Console(
					theme: AnsiConsoleTheme.Code,
					outputTemplate:
						"[{Timestamp:HH:mm:ss} {Level:u3}] [App={Application}] [User={UserId}] [Corr={CorrelationId}] {Message:lj}{NewLine}{Exception}"
				)
				.WriteTo.File(
					"Logs/log-.txt",
					rollingInterval: RollingInterval.Day,
					retainedFileCountLimit: 15,
					shared: true,
					outputTemplate:
						"[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [App={Application}] [User={UserId}] [Corr={CorrelationId}] {Message:lj}{NewLine}{Exception}"
				)
				.CreateLogger();

			builder.Host.UseSerilog();
		}
	}
}

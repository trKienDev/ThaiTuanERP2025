using Microsoft.Extensions.Logging;
using ThaiTuanERP2025.Application.Common.Services;

namespace ThaiTuanERP2025.Infrastructure.Common.Services
{
	/// <summary>
	/// Triển khai logging service bằng Serilog.
	/// Tầng Infrastructure có thể thay đổi logging engine mà không ảnh hưởng Application.
	/// </summary>
	public sealed class SerilogLoggingService : ILoggingService
	{
		private readonly ILogger _logger;

		public SerilogLoggingService(ILogger logger)
		{
			_logger = logger;
		}

		public void LogInformation(string message, params object[] args)
		    => _logger.LogInformation(message, args);

		public void LogWarning(string message, params object[] args)
		    => _logger.LogWarning(message, args);

		public void LogError(Exception exception, string message, params object[] args)
		    => _logger.LogError(exception, message, args);

		public void LogDebug(string message, params object[] args)
		    => _logger.LogDebug(message, args);
	}
}

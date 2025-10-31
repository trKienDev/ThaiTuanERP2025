namespace ThaiTuanERP2025.Application.Common.Services
{
	public interface ILoggingService
	{
		void LogInformation(string message, params object[] args);
		void LogWarning(string message, params object[] args);
		void LogError(Exception exception, string message, params object[] args);
		void LogDebug(string message, params object[] args);
	}
}

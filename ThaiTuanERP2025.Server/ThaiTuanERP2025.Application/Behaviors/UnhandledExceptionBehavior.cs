using MediatR;
using Microsoft.Extensions.Logging;

namespace ThaiTuanERP2025.Application.Behaviors
{
	public sealed class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;

		public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
		{
			_logger = logger;
		}

		public async Task<TResponse> Handle(
			TRequest request,
			RequestHandlerDelegate<TResponse> next,
			CancellationToken cancellationToken)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				var requestName = typeof(TRequest).Name;
				_logger.LogError(ex, "❌ Unhandled Exception for Request {RequestName} {@Request}", requestName, request);

				// Bạn có thể thêm gửi alert qua NotificationService, Slack, SignalR,... tại đây

				throw; // rethrow để ExceptionHandlingMiddleware xử lý tiếp
			}
		}
	}
}

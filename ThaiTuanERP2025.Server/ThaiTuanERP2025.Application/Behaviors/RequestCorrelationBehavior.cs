using MediatR;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Application.Shared.Services;

namespace ThaiTuanERP2025.Application.Behaviors
{
	/// <summary>
	/// Gắn CorrelationId (định danh duy nhất) cho mỗi request.
	/// </summary>
	public sealed class RequestCorrelationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
	where TRequest : IRequest<TResponse>
	{
		private readonly ICorrelationIdProvider _correlationIdProvider;
		private readonly ILoggingService _logger;

		public RequestCorrelationBehavior(ICorrelationIdProvider correlationIdProvider, ILoggingService logger)
		{
			_correlationIdProvider = correlationIdProvider;
			_logger = logger;
		}

		public async Task<TResponse> Handle(
			TRequest request,
			RequestHandlerDelegate<TResponse> next,
			CancellationToken cancellationToken)
		{
			var correlationId = _correlationIdProvider.GetCorrelationId();
			var requestName = typeof(TRequest).Name;

			_logger.LogInformation("➡ Handling {RequestName} (CorrelationId: {CorrelationId})", requestName, correlationId);

			var response = await next();

			_logger.LogInformation("✅ Completed {RequestName} (CorrelationId: {CorrelationId})", requestName, correlationId);

			return response;
		}
	}
}

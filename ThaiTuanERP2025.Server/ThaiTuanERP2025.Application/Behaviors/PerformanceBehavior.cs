using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ThaiTuanERP2025.Application.Behaviors
{
	public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
		private readonly Stopwatch _timer;

		public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
		{
			_logger = logger;
			_timer = new Stopwatch();
		}

		public async Task<TResponse> Handle(
			TRequest request,
			RequestHandlerDelegate<TResponse> next,
			CancellationToken cancellationToken)
		{
			_timer.Start();

			var response = await next();

			_timer.Stop();

			var elapsed = _timer.ElapsedMilliseconds;

			if (elapsed > 500) // >500ms thì log cảnh báo
			{
				var requestName = typeof(TRequest).Name;
				_logger.LogWarning(
					"🐢 Long Running Request: {RequestName} ({Elapsed} ms) {@Request}",
					requestName, elapsed, request);
			}

			return response;
		}
	}
}

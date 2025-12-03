using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ThaiTuanERP2025.Application.Shared.Interfaces;

namespace ThaiTuanERP2025.Application.Behaviors
{
	public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
                private readonly ICurrentUserService _currentUser;
                private readonly Stopwatch _timer = new();

		public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_timer = new Stopwatch();
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			_timer.Start();

			var response = await next();

			_timer.Stop();

			var elapsed = _timer.ElapsedMilliseconds;

			if (elapsed > 500) // >500ms thì log cảnh báo
			{
				var requestName = typeof(TRequest).Name;
					_logger.LogWarning(
					"🐢 Long Running Request: {RequestName} took {Elapsed} ms (UserId={UserId})",
					requestName,
					elapsed,
					_currentUser.UserId
				);
                        }

			return response;
		}
	}
}

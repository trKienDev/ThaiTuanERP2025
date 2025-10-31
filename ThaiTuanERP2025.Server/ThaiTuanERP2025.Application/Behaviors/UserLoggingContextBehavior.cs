using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Behaviors
{
	/// <summary>
	/// Thêm thông tin UserId vào LogContext của Serilog
	/// </summary>
	public sealed class UserLoggingContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ICurrentUserService _currentUserService;

		public UserLoggingContextBehavior(ICurrentUserService currentUserService)
		{
			_currentUserService = currentUserService;
		}

		public async Task<TResponse> Handle(
		    TRequest request,
		    RequestHandlerDelegate<TResponse> next,
		    CancellationToken cancellationToken)
		{
			var userId = _currentUserService.UserId;

			// Nếu có user đăng nhập → thêm vào LogContext
			using (LogContext.PushProperty("UserId", userId ?? "Anonymous"))
			{
				return await next();
			}
		}
	}
}

using MediatR;
using ThaiTuanERP2025.Application.Authentication.Commands.Login;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Exceptions;

namespace ThaiTuanERP2025.Application.Behaviors
{
	/// <summary>
	/// Thêm thông tin UserId vào LogContext của Serilog
	/// </summary>
	public sealed class UserLoggingContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly ILoggingService _logger;

		public UserLoggingContextBehavior(ICurrentUserService currentUserService, ILoggingService logger)
		{
			_currentUserService = currentUserService;
			_logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			// Nếu là lệnh Login / Refresh thì bỏ qua kiểm tra UserId
			var requestName = typeof(TRequest).Name;
			if (request is LoginCommand || requestName.Contains("Login") || requestName.Contains("Refresh"))
			{
				return await next();
			}

			var userId = _currentUserService.UserId ?? throw new NotFoundException("User không hợp lệ");

			_logger.LogInformation("➡ Handling {RequestName} by User: {UserId}", requestName, userId);

			var response = await next();

			_logger.LogInformation("✅ Completed {RequestName} by User: {UserId}", requestName, userId);

			return response;
		}
	}
}

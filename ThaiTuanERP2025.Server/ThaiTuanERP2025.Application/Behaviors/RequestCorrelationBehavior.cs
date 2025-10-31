using MediatR;

namespace ThaiTuanERP2025.Application.Behaviors
{
	/// <summary>
	/// Gắn CorrelationId (định danh duy nhất) cho mỗi request.
	/// </summary>
	public sealed class RequestCorrelationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public RequestCorrelationBehavior(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<TResponse> Handle(
		    TRequest request,
		    RequestHandlerDelegate<TResponse> next,
		    CancellationToken cancellationToken)
		{
			const string key = "X-Correlation-ID";

			string? correlationId = null;

			// Lấy từ HttpContext.Items nếu có (do middleware set)
			var context = _httpContextAccessor.HttpContext;
			if (context != null && context.Items.ContainsKey(key))
				correlationId = context.Items[key]?.ToString();

			// Nếu không có (vd background job), thì tự tạo mới
			correlationId ??= Guid.NewGuid().ToString("N");

			using (LogContext.PushProperty("CorrelationId", correlationId))
			{
				return await next();
			}
		}
	}
}

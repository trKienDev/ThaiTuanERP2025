using System.Diagnostics;

namespace ThaiTuanERP2025.Api.Middleware
{
	/// <summary>
	/// Middleware để gắn hoặc đọc X-Correlation-ID trong mỗi HTTP request.
	/// </summary>
	public sealed class CorrelationIdMiddleware
	{
		private const string HeaderKey = "X-Correlation-ID";
		private readonly RequestDelegate _next;

		public CorrelationIdMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Đọc từ header nếu có, ngược lại tự tạo mới
			var correlationId = context.Request.Headers[HeaderKey].FirstOrDefault();

			if (string.IsNullOrWhiteSpace(correlationId))
			{
				correlationId = Guid.NewGuid().ToString("N");
				context.Request.Headers[HeaderKey] = correlationId; // thêm vào header cho response middleware khác dùng
			}

			// Ghi vào HttpContext.Items (để tầng Application đọc lại)
			context.Items[HeaderKey] = correlationId;

			// Thêm vào response header (để client trace)
			context.Response.OnStarting(() =>
			{
				context.Response.Headers[HeaderKey] = correlationId;
				return Task.CompletedTask;
			});

			// Ghi vào Activity.Current (nếu dùng OpenTelemetry sau này)
			Activity.Current?.AddTag(HeaderKey, correlationId);

			await _next(context);
		}
	}
}

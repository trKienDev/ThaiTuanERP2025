using Serilog;

namespace ThaiTuanERP2025.Api.Middleware
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public RequestLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var method = context.Request.Method;
			var path = context.Request.Path;
			var user = context.User.Identity?.Name ?? "Anonymous";

			Log.Information("🌐 HTTP {Method} {Path} by {User}", method, path, user);

			await _next(context);
		}
	}
}

﻿namespace ThaiTuanERP2025.Api.Middleware
{
	public static class CorrelationIdMiddlewareExtensions
	{
		public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
		{
			return app.UseMiddleware<CorrelationIdMiddleware>();
		}
	}
}

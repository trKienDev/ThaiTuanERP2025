using ThaiTuanERP2025.Application.Shared.Interfaces;

namespace ThaiTuanERP2025.Api.Services
{
	public sealed class CorrelationIdProvider : ICorrelationIdProvider
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private const string HeaderKey = "X-Correlation-ID";

		public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string GetCorrelationId()
		{
			var context = _httpContextAccessor.HttpContext;
			if (context == null)
				return Guid.NewGuid().ToString("N");

			if (context.Items.ContainsKey(HeaderKey))
				return context.Items[HeaderKey]?.ToString() ?? Guid.NewGuid().ToString("N");

			if (context.Request.Headers.TryGetValue(HeaderKey, out var headerValue))
				return headerValue.ToString();

			return Guid.NewGuid().ToString("N");
		}
	}
}

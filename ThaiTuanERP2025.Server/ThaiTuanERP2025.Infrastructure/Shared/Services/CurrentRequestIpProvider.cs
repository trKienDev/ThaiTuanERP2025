using Microsoft.AspNetCore.Http;
using ThaiTuanERP2025.Application.Shared.Services;

namespace ThaiTuanERP2025.Infrastructure.Shared.Services
{
	/// <summary>
	/// Triển khai ICurrentRequestIpProvider dùng IHttpContextAccessor để lấy IP của request hiện tại.
	/// </summary>
	public sealed class CurrentRequestIpProvider : ICurrentRequestIpProvider
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CurrentRequestIpProvider(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string? GetIp()
		{
			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext is null)
				return null;

			// Nếu có header X-Forwarded-For (chạy qua reverse proxy)
			var forwardedHeader = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
			if (!string.IsNullOrWhiteSpace(forwardedHeader))
				return forwardedHeader.Split(',').FirstOrDefault()?.Trim();

			// Lấy trực tiếp IP từ connection
			return httpContext.Connection.RemoteIpAddress?.ToString();
		}
	}
}

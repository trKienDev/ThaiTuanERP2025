using System.Net;

namespace ThaiTuanERP2025.Application.Shared.Exceptions
{
	public class UnauthorizedException : AppException
	{
		public UnauthorizedException(string message = "Bạn không có quyền truy cập") : base(message, (int)HttpStatusCode.Unauthorized) { }
	}
}

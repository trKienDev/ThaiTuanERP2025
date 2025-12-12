using System.Net;

namespace Drive.Application.Shared.Exceptions
{
	public class NotFoundException : AppException
	{
		public NotFoundException(string message = "Không tìm thấy dữ liệu") : base(message, (int)HttpStatusCode.NotFound) { }
	}
}

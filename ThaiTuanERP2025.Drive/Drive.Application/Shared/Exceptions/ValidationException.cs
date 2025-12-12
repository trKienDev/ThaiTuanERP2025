using System.Net;

namespace Drive.Application.Shared.Exceptions
{
	public class ValidationException : AppException
	{
		public ValidationException(string message = "Lỗi xác thực") : base(message, (int)HttpStatusCode.NotFound) { }
	}
}

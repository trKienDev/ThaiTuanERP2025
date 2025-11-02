using System.Net;

namespace ThaiTuanERP2025.Application.Exceptions
{
	/// <summary>
	/// Dùng cho lỗi validate input. Chứa danh sách lỗi theo từng field.
	/// </summary>
	public class ValidationException : AppException
	{
		public ValidationException(string message = "Lỗi xác thực") : base(message, (int)HttpStatusCode.NotFound) { }
	}
}

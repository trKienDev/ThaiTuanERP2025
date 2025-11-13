using System.Net;

namespace ThaiTuanERP2025.Application.Shared.Exceptions
{
	public class ConflictException : AppException
	{
		public ConflictException(string message) : base(message, (int)HttpStatusCode.Conflict) { }
	}
}

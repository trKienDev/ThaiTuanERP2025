using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Exceptions
{
	public class ForbiddenException : AppException
	{
		public ForbiddenException(string message = "Bạn không có quyền truy cập") : base(message, (int)HttpStatusCode.Forbidden) { }
	}
}

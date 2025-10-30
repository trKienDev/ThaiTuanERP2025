﻿using System.Net;

namespace ThaiTuanERP2025.Domain.Exceptions
{
	public class ForbiddenException : AppException
	{
		public ForbiddenException(string message = "Bạn không có quyền truy cập") : base(message, (int)HttpStatusCode.Forbidden) { }
	}
}

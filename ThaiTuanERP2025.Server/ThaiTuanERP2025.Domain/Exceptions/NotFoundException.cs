using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Exceptions
{
	public class NotFoundException : AppException
	{
		public NotFoundException(string message = "Không tìm thấy dữ liệu") : base(message, (int)HttpStatusCode.NotFound) { }
	}
}

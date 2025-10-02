using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Exceptions
{
	public class ConflictException : AppException
	{
		public ConflictException(string message) : base(message, (int)HttpStatusCode.Conflict) { }
	}
}

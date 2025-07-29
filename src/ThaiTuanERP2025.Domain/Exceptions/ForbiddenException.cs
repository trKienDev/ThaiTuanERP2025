using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Exceptions
{
	public class ForbiddenException : Exception
	{
		public ForbiddenException() : base() { }
		public ForbiddenException(string message) : base(message) { }
		public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
	}
}

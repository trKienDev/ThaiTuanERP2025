using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Common.Interfaces
{
	public interface ICurrentUserService
	{
		Guid? UserId { get; }
	}
}

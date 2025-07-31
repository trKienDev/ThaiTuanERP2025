using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public record DepartmentDto(
		Guid Id,
		string Name, 
		string Code
	);
}

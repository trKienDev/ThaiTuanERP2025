using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public class CreateGroupDto
	{
		public string Name { get; set; } = string.Empty;	
		public string Description { get; set; } = string.Empty;
		public Guid AdminUserId { get; set; }
	}
}

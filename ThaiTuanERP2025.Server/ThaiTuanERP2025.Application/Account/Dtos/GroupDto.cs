using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public class GroupDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public Guid AdminId { get; set; }	
		public string AdminName { get; set; } = string.Empty;
		public int MemberCount { get; set; } = 0;
		public List<UserDto> Members { get; set; } = new();
	}
}

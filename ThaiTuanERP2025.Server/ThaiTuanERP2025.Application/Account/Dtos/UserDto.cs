using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Enums;

namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public class UserDto
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public string EmployeeCode { get; set; } = default!;
		public string Position { get; set; } = default!;
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string AvatarUrl { get; set; } = default!;

		public UserRole Role { get; set; } = default!;
		public Guid? DepartmentId { get; set; }

		public DepartmentDto? Department { get; set; } = default!;
	}

	
}
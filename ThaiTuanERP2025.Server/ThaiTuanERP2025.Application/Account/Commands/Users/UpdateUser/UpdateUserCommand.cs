using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUser
{
	public class UpdateUserCommand : IRequest<UserDto>
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = default!;
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string AvatarUrl { get; set; } = string.Empty;
		public string Position { get; set; } = default!;
		public string Role { get; set; } = default!;
		public Guid DepartmentId { get; set; }
		public bool IsActive { get; set; }
	}
}

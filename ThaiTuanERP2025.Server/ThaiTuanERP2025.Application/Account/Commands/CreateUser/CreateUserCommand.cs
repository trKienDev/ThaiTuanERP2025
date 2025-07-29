using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Commands.CreateUser
{
	public class CreateUserCommand : IRequest<UserDto>
	{
		public string FullName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public string EmployeeCode { get; set; } = default!;
		public string Password { get; set; } = default!;
		public string AvatarUrl { get; set; } = string.Empty;
		public string Role {  get; set; } = default!;
		public string Position { get; set; } = default!;
		public Guid DepartmentId { get; set; }

		public string? Email {  get; set; }
		public string? Phone {  get; set; }
	}
}

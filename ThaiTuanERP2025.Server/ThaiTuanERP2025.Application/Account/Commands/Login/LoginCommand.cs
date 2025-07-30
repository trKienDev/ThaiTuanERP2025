using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Enums;

namespace ThaiTuanERP2025.Application.Account.Commands.Login
{
	public class LoginCommand : IRequest<LoginResultDto>
	{
		public string Username { get; set; } = default!;
		public string Password { get; set; } = default!;
	}

	public class LoginResultDto {
		public string AccessToken { get; set; } = default!;
		public UserRole UserRole { get; set; }
	}
}

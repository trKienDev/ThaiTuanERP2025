using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.ChangePassword
{
	public class ChangePasswordCommand : IRequest<string>
	{
		public Guid UserId { get; set; }
		public string CurrentPassword { get; set; } = default!;
		public string NewPassword { get; set; } = default!;
		public string ConfirmPassword { get; set; } = default!;
	}
}

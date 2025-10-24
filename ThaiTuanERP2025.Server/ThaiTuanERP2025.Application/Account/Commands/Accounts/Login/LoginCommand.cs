using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Login
{
	public class LoginCommand : IRequest<LoginResultDto>
	{
		public string EmployeeCode { get; set; } = default!;
		public string Password { get; set; } = default!;
	}

	public class LoginResultDto {
		public string AccessToken { get; set; } = default!;
		public string FullName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public List<string> Roles { get; set; } = new();
		public List<string> Permissions { get; set; } = new();
	}
}

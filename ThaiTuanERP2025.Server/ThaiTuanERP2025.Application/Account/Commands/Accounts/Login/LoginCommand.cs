using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Commands.Login
{
	public sealed record LoginCommand : IRequest<LoginResponseDto>
	{
		public string EmployeeCode { get; set; } = default!;
		public string Password { get; set; } = default!;
	}
}

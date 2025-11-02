using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Requests;

namespace ThaiTuanERP2025.Application.Account.Users.Commands.Create
{
	public sealed record CreateUserCommand(UserRequest Request) : IRequest<Unit>;
}

using MediatR;
using ThaiTuanERP2025.Application.Account.Users.Requests;

namespace ThaiTuanERP2025.Application.Account.Users.Commands.SetAvatar
{
	public sealed record SetUserAvatarCommand(Guid UserId, SetUserAvatarRequest Request) : IRequest<Unit>;
}

using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUserAvatarFileId
{
	public sealed record UpdateUserAvatarFileIdCommand(Guid UserId, Guid FileId) : IRequest;
}

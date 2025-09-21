using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.UpdateGroup
{
	public record UpdateGroupCommand(
		Guid GroupId,
		string NewName,
		string NewDescription,
		Guid RequestingUserId
	) : IRequest;
}

using MediatR;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.AddUserToGroup
{
	public record AddUserToGroupCommand(
		Guid GroupId,
		Guid UserId
	) : IRequest;
}

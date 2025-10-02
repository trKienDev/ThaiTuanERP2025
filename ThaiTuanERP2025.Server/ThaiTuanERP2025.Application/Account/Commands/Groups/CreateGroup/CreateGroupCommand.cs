using MediatR;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Commands.Group.CreateGroup
{
	public record CreateGroupCommand(
		string Name,
		string Description,
		string Slug,
		Guid AdminUserId
	) : IRequest<GroupDto>;
}

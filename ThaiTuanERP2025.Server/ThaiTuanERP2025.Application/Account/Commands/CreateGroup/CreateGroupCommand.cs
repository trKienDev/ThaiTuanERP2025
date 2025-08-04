using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Commands.CreateGroup
{
	public record CreateGroupCommand(
		string Name,
		string Description,
		Guid AdminUserId
	) : IRequest<GroupDto>;
}

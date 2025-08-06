using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.AddUserToGroup
{
	public record AddUserToGroupCommand(
		Guid GroupId,
		Guid UserId
	) : IRequest;
}

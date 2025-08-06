using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.Groups.UpdateGroup
{
	public record UpdateGroupCommand(
		Guid GroupId,
		string NewName,
		string NewDescription,
		Guid RequestingUserId
	) : IRequest;
}

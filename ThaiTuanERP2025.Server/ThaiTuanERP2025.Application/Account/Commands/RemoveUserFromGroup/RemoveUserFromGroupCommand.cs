using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.RemoveUserFromGroup
{
	public record RemoveUserFromGroupCommand(
		Guid GroupId,
		Guid TargetUserId,
		Guid RequestingUserId
	) : IRequest;
}

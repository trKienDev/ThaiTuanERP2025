using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Account.Commands.ChangeGroupAdmin
{
	public record ChangeGroupAdminCommand(
		Guid GroupId,
		Guid TargetUserId,
		Guid RequestorId
	) : IRequest;
}

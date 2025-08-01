using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Queries.GetCurrentUser
{
	public class GetCurrentUserQuery : IRequest<UserDto>
	{
		public ClaimsPrincipal UserPrincipal { get; }
		public GetCurrentUserQuery(ClaimsPrincipal userPrincipal)
		{
			UserPrincipal = userPrincipal;
		}
	}
}

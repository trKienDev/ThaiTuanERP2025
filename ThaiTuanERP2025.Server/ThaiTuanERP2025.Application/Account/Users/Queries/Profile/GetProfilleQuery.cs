using MediatR;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Account.Dtos;

namespace ThaiTuanERP2025.Application.Account.Users.Queries.Profile
{
	public sealed class GetProfilleQuery : IRequest<UserDto>
	{
		public ClaimsPrincipal UserPrincipal { get; }
		public GetProfilleQuery(ClaimsPrincipal userPrincipal)
		{
			UserPrincipal = userPrincipal;
		}
	}
}

using System.Security.Claims;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Shared.Authentication
{
	public interface IJWTProvider
	{
		string GenerateToken(User user, IEnumerable<Claim> claims);
	}
}

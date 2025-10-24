using System.Security.Claims;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Authentication
{
	public interface iJWTProvider
	{
		string GenerateToken(User user, IEnumerable<Claim> claims);
	}
}

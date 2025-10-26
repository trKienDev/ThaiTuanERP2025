using System.Security.Claims;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Common.Authentication
{
	public interface IJWTProvider
	{
		string GenerateToken(User user, IEnumerable<Claim> claims);
	}
}

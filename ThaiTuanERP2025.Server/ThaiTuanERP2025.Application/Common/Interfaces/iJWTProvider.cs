using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Common.Interfaces
{
	public interface iJWTProvider
	{
		string GenerateToken(User user);
	}
}

using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Authentication.Entities
{
	public class RefreshToken
	{
		private RefreshToken() { } // EF

		public RefreshToken(Guid userId, string tokenHash, DateTime expiresAt, string? createdByIp = null)
		{
			Id = Guid.NewGuid();
			UserId = userId;
			TokenHash = tokenHash;
			ExpiresAt = expiresAt;
			CreatedByIp = createdByIp;
			IsRevoked = false;
		}

		public Guid Id { get; private set; } = Guid.NewGuid();
		public Guid UserId { get; private set; }
		public string TokenHash { get; private set; } = null!;        // SHA256(token)
		public DateTime ExpiresAt { get; private set; }
		public bool IsRevoked { get; private set; }
		public DateTime? RevokedAt { get; private set; }
		public string? RevokedByIp { get; private set; }
		public string? ReplacedByTokenHash { get; private set; }      // rotation chain
		public string? CreatedByIp { get; private set; }

		public bool IsExpired => DateTime.UtcNow > ExpiresAt;
		public bool IsActive => !IsRevoked && !IsExpired;

		public void Revoke(string? ip = null)
		{
			if (IsRevoked) return;
			IsRevoked = true;
			RevokedAt = DateTime.UtcNow;
			RevokedByIp = ip;
		}

		public void ReplaceBy(string newTokenHash)
		{
			ReplacedByTokenHash = newTokenHash;
			Revoke();
		}
	}
}

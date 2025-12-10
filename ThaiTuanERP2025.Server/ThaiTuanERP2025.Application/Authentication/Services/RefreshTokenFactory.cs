using System.Security.Cryptography;
using System.Text;

namespace ThaiTuanERP2025.Application.Authentication.Services
{
	public static class RefreshTokenFactory
	{
		public static string GenerateSecureToken(int bytesLength = 64)
		{
			var bytes = RandomNumberGenerator.GetBytes(bytesLength);
			return Convert.ToBase64String(bytes); // trả plain token cho client
		}

		public static string ComputeSha256(string input)
		{
			using var sha = SHA256.Create();
			var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
			return Convert.ToHexString(hash); // 64 bytes hex string
		}
	}
}

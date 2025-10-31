using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThaiTuanERP2025.Application.Common.Authentication;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Common.Security
{
	public sealed class JwtProvider : IJWTProvider
	{
		private readonly JwtSettings _settings;

		public JwtProvider(IOptions<JwtSettings> options)
		{
			_settings = options.Value;
		}

		public string GenerateToken(User user, IEnumerable<Claim> claims)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var tokenClaims = new List<Claim>(claims)
			{
				new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new(JwtRegisteredClaimNames.UniqueName, user.Username),
				new("fullName", user.FullName),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var token = new JwtSecurityToken(
				issuer: _settings.Issuer,
				audience: _settings.Audience,
				claims: tokenClaims,
				expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}

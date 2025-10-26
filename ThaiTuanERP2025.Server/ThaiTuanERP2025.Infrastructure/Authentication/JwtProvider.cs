using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThaiTuanERP2025.Application.Common.Authentication;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Authentication
{
	public class JwtProvider : IJWTProvider
	{
		private readonly IConfiguration _configuration;
		public JwtProvider(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <summary>
		/// Sinh JWT Token dựa trên danh sách claims (bao gồm Role + Permission)
		/// </summary>
		public string GenerateToken(User user, IEnumerable<Claim> claims)
		{ 
			var jwtSettings = _configuration.GetSection("Jwt");
			var secretkey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Secret Key is missing.");
			var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing.");
			var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is missing.");
			var expiresInMinutesStr = jwtSettings["ExpiresInMinutes"] ?? "60";
			var expiresInMinutes = int.Parse(expiresInMinutesStr);

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var baseClaims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
				new Claim("fullName", user.FullName)
			};

			var allClaims = new List<Claim>(baseClaims);
			if (claims != null)
				allClaims.AddRange(claims);

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: allClaims,
				expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}

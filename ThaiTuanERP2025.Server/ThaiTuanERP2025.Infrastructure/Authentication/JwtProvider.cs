using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Authentication
{
	public class JwtProvider : iJWTProvider
	{
		private readonly IConfiguration _configuration;
		public JwtProvider(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(User user) {
			var jwtSettings = _configuration.GetSection("Jwt");
			var secretkey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Secret Key is missing.");
			var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is missing.");
			var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is missing.");
			var expiresInMinutesStr = jwtSettings["ExpiresInMinutes"] ?? "60";
			var expiresInMinutes = int.Parse(expiresInMinutesStr);

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[] {
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
				new Claim(ClaimTypes.Name, user.FullName),
				new Claim(ClaimTypes.Role, user.Role.ToString())
			};


			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}

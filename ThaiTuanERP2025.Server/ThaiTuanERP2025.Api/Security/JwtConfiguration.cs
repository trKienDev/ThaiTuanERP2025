using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ThaiTuanERP2025.Api.Security
{
	public static class JwtConfiguration
	{
		public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtSection = configuration.GetSection("Jwt");
			var secretKey = jwtSection["Key"];
			var issuer = jwtSection["Issuer"];
			var audience = jwtSection["Audience"];

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			    .AddJwtBearer(options =>
			    {
				    options.TokenValidationParameters = new TokenValidationParameters
				    {
					    ValidateIssuer = true,
					    ValidateAudience = true,
					    ValidateLifetime = true,
					    ValidateIssuerSigningKey = true,

					    ValidIssuer = issuer,
					    ValidAudience = audience,
					    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

					    NameClaimType = ClaimTypes.NameIdentifier,
					    RoleClaimType = ClaimTypes.Role,
					    ClockSkew = TimeSpan.Zero // không trễ thời gian
				    };

				    // Cho phép SignalR đọc token qua query string
				    options.Events = new JwtBearerEvents
				    {
					    OnMessageReceived = context =>
					    {
						    var accessToken = context.Request.Query["access_token"];
						    var path = context.HttpContext.Request.Path;
						    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
						    {
							    context.Token = accessToken;
						    }
						    return Task.CompletedTask;
					    }
				    };
			    });

			return services;
		}
	}
}

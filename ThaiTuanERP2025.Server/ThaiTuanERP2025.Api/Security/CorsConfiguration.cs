namespace ThaiTuanERP2025.Api.Security
{
	public static class CorsConfiguration
	{
		public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
		{
			// Lấy danh sách origin từ appsettings hoặc environment variable
			var allowedOrigins = configuration
			    .GetSection("Cors:AllowedOrigins")
			    .Get<string[]>() ?? new[] { "http://localhost:4200" };

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(policy =>
				{
					policy
					    .WithOrigins(allowedOrigins)
					    .AllowAnyMethod()
					    .AllowAnyHeader()
					    .AllowCredentials();
				});

			});

			return services;
		}
	}
}

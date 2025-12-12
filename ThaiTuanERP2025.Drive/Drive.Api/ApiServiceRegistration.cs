using Drive.Api.Security;

namespace Drive.Api
{
	public static class ApiServiceRegistration
	{
		public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration) {
			services.AddCorsPolicy(configuration);

			return services;
		}
	}
}

using Microsoft.Extensions.DependencyInjection;

namespace Drive.Application.DependencyInjection
{
	public static class ApplicationServiceRegistration
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly));

			services.AddAutoMapper(typeof(ApplicationServiceRegistration).Assembly);

			return services;
		}
	}
}

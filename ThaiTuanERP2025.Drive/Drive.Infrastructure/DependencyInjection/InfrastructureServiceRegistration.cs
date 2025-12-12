using Drive.Application.Abstractions;
using Drive.Domain.Repositories;
using Drive.Infrastructure.FileStorage;
using Drive.Infrastructure.Persistence;
using Drive.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Drive.Infrastructure.DependencyInjection
{
	public static class InfrastructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<ThaiTuanERP2025DriveDbContext>(options => options.UseSqlServer(config.GetConnectionString("StorageDb")));

			services.AddScoped<IFileDriveProvider, StoredObjectProvider>();

			services.AddScoped<IStoredObjectWriteRepository, StoredObjectWriteRepository>();

			return services;
		}
	}
}

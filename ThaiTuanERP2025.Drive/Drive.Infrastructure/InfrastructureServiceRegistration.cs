using Drive.Application.Abstractions;
using Drive.Application.Shared.Interfaces;
using Drive.Domain.Repositories;
using Drive.Infrastructure.FileStorage;
using Drive.Infrastructure.Persistence;
using Drive.Infrastructure.Repositories;
using Drive.Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Drive.Infrastructure
{
	public static class InfrastructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<ThaiTuanERP2025DriveDbContext>(options => options.UseSqlServer(config.GetConnectionString("ThaiTuanERP2025DriveDb")));

			services.AddScoped<IFileDriveProvider, StoredObjectProvider>();

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IStoredObjectWriteRepository, StoredObjectWriteRepository>();

			return services;
		}
	}
}

using Drive.Application.Abstractions;
using Drive.Application.Interfaces;
using Drive.Application.Shared.Interfaces;
using Drive.Domain;
using Drive.Domain.Repositories;
using Drive.Infrastructure.FileStorage;
using Drive.Infrastructure.Persistence;
using Drive.Infrastructure.Repositories;
using Drive.Infrastructure.Services;
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

			services.AddOptions<ObjectStorageOptions>()
				.Bind(config.GetSection(ObjectStorageOptions.SectionName))
				.Validate(o => !string.IsNullOrWhiteSpace(o.BasePath),"ObjectStorage.BasePath is required")
				.ValidateOnStart();

			services.AddScoped<IFileDriveProvider, StoredObjectProvider>();

			services.AddScoped<IObjectStorage, ObjectStorageService>();

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IStoredObjectWriteRepository, StoredObjectWriteRepository>();

			return services;
		}
	}
}

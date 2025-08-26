using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Infrastructure.Files.FileStorage;

namespace ThaiTuanERP2025.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
		{
			services.AddMinio(c => c
				.WithEndpoint(new Uri(cfg["Minio:Endpoint"]!))      // ví dụ: http://localhost:9000
				.WithCredentials(cfg["Minio:AccessKey"]!, cfg["Minio:SecretKey"]!)
				.Build()
			);
			services.AddScoped<IFileStorage, MinioFileStorage>();

			return services;
		}
	}
}

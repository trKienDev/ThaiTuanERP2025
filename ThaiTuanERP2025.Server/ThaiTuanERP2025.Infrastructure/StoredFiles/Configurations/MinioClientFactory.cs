using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations
{
	public static class CustomMinioClientFactory
	{
		public static IMinioClient Create(MinioSettings settings) {
			if(string.IsNullOrWhiteSpace(settings.Endpoint))
				throw new ArgumentNullException("MinIO Endpoint is not configured");
			if (string.IsNullOrWhiteSpace(settings.AccessKey) || string.IsNullOrWhiteSpace(settings.SecretKey))
				throw new ArgumentNullException("Minio credentials are missing");

			var uri = new Uri(settings.Endpoint);
			return new MinioClient().WithEndpoint(uri.Host, uri.Port)
				.WithCredentials(settings.AccessKey, settings.SecretKey)	
				.WithSSL(uri.Scheme == "https" || settings.UseSSL)
				.Build();
		}
	}
}

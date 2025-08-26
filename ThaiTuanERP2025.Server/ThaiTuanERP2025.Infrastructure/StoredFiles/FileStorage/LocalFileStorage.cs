using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.FileStorage
{
	public sealed class LocalFileStorage : IFileStorage, IFileStorageInfo
 	{
		private readonly string _basePath;
		private readonly int _expirySeconds;
		public string BucketName => "local-files";
		public LocalFileStorage(IOptions<FileStorageOptions> options) {
			_basePath = options.Value.BasePath ?? "D:\\ERP-Files";
			_expirySeconds = options.Value.PresignedExpirySeconds;
		}

		public Task EnsureReadyAsync(CancellationToken cancellationToken)
		{
			Directory.CreateDirectory(_basePath);
			return Task.CompletedTask;
		}

		public async Task UploadAsync(string objectKey, Stream content, string conentType, CancellationToken cancellationToken) {
			var path = Path.Combine(_basePath, objectKey.Replace("/", "\\"));
			Directory.CreateDirectory(Path.GetDirectoryName(path)!);
			using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);	
			await content.CopyToAsync(fileStream, cancellationToken);
		}

		public Task<string> GetPresignedGetUrlAsync(string objectKey, CancellationToken cancellationToken)
		{
			var url = $"/files/public/{objectKey}";
			return Task.FromResult(url);
		}

		public Task RemoveAsync(string objectKey, CancellationToken cancellationToken)
		{
			var path = Path.Combine(_basePath, objectKey.Replace("/", "\\"));
			if(File.Exists(path))
				File.Delete(path);
			return Task.CompletedTask;
		}
	}
}

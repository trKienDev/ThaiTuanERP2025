using Drive.Application.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Drive.Infrastructure.FileStorage
{
	public sealed class StoredObjectProvider : IFileDriveProvider
	{
		private readonly string _basePath;
		public StoredObjectProvider(IConfiguration config)
		{
			_basePath = config["FileDrive:BasePath"] ?? throw new Exception("Missing FileDrive:BasePath config");
		}

		public async Task SaveAsync(string bucket, string objectKey, Stream stream, string contentType, CancellationToken cancellationToken = default)
		{
			var fullPath = Path.Combine(_basePath, bucket, objectKey);
			var dir = Path.GetDirectoryName(fullPath)!;

			Directory.CreateDirectory(dir);

			using var file = File.Create(fullPath);
			await stream.CopyToAsync(file, cancellationToken);
		}

		public Task<Stream> OpenReadAsync(string bucket, string objectKey, CancellationToken cancellationToken = default)
		{
			var fullPath = Path.Combine(_basePath, bucket, objectKey);
			Stream s = File.OpenRead(fullPath);
			return Task.FromResult(s);
		}

		public Task<bool> ExistsAsync(string bucket, string objectKey, CancellationToken cancellationToken = default)
		{
			var fullPath = Path.Combine(_basePath, bucket, objectKey);
			return Task.FromResult(File.Exists(fullPath));
		}

		public Task DeleteAsync(string bucket, string objectKey, CancellationToken cancellationToken = default)
		{
			var fullPath = Path.Combine(_basePath, bucket, objectKey);
			if (File.Exists(fullPath)) File.Delete(fullPath);
			return Task.CompletedTask;
		}
	}
}

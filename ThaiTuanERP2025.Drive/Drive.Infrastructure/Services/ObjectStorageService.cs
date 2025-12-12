using Drive.Application.Interfaces;
using Drive.Domain;
using Microsoft.Extensions.Options;

namespace Drive.Infrastructure.Services
{
	public sealed class ObjectStorageService : IObjectStorage
	{
		private readonly IOptionsMonitor<ObjectStorageOptions> _opts;

		public ObjectStorageService(IOptionsMonitor<ObjectStorageOptions> opts)
		    => _opts = opts;

		public Task EnsureReadyAsync(CancellationToken cancellationToken)
		{
			var basePath = _opts.CurrentValue.BasePath;

			if (string.IsNullOrWhiteSpace(basePath))
				throw new InvalidOperationException("ObjectStorage.BasePath is not configured");

			Directory.CreateDirectory(basePath);
			return Task.CompletedTask;
		}

		public async Task UploadAsync(string objectKey, Stream content, string contentType, CancellationToken cancellationToken)
		{
			var basePath = _opts.CurrentValue.BasePath;
			var path = Path.Combine(basePath, objectKey.Replace("/", Path.DirectorySeparatorChar.ToString()));
			Directory.CreateDirectory(Path.GetDirectoryName(path)!);
			using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			await content.CopyToAsync(fileStream, cancellationToken);
		}

		public Task<string> GetPresignedGetUrlAsync(string objectKey, CancellationToken cancellationToken)
		{
			var reqPath = _opts.CurrentValue.PublicRequestPath?.TrimEnd('/') ?? "/files/public";
			var url = $"{reqPath}/{objectKey.Replace("\\", "/")}";
			return Task.FromResult(url);
		}

		public Task<Stream> OpenReadStreamAsync(string objectKey, CancellationToken cancellationToken)
		{
			var path = Path.Combine(_opts.CurrentValue.BasePath, objectKey);
			Stream stream = File.OpenRead(path);
			return Task.FromResult(stream);
		}

		public Task RemoveAsync(string objectKey, CancellationToken cancellationToken)
		{
			var basePath = _opts.CurrentValue.BasePath;
			var path = Path.Combine(basePath, objectKey.Replace("/", Path.DirectorySeparatorChar.ToString()));
			if (File.Exists(path)) File.Delete(path);
			return Task.CompletedTask;
		}
	}
}

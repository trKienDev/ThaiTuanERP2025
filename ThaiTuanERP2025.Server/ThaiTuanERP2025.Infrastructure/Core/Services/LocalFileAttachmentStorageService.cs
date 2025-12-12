using Microsoft.Extensions.Options;
using ThaiTuanERP2025.Application.Core.FileAttachments.Services;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Core.Services
{
	public sealed class LocalFileAttachmentStorageService : IFileAttachmentStorageService
	{
		private readonly IOptionsMonitor<FileStorageOptions> _opts;

		public LocalFileAttachmentStorageService(IOptionsMonitor<FileStorageOptions> opts)
		    => _opts = opts;

		public Task EnsureReadyAsync(CancellationToken cancellationToken)
		{
			Directory.CreateDirectory(_opts.CurrentValue.BasePath);
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
			Stream stream = File .OpenRead(path);
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

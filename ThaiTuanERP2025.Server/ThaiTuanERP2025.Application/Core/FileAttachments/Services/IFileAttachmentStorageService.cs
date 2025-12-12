namespace ThaiTuanERP2025.Application.Core.FileAttachments.Services
{
	public interface IFileAttachmentStorageService
	{
		Task EnsureReadyAsync(CancellationToken cancellationToken);
		Task UploadAsync(string objectKey, Stream content, string contentType, CancellationToken cancellationToken);
		Task<string> GetPresignedGetUrlAsync(string objectKey, CancellationToken cancellationToken);
		Task RemoveAsync(string objectKey, CancellationToken cancellationToken);
		Task<Stream> OpenReadStreamAsync(string objectKey, CancellationToken cancellationToken);
	}
}

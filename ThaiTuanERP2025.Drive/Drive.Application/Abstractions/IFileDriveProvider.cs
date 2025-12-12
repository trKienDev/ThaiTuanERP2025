namespace Drive.Application.Abstractions
{
	public interface IFileDriveProvider
	{
		Task SaveAsync(string bucket, string objectKey, Stream stream, string contentType, CancellationToken cancellationToken = default);
		Task<Stream> OpenReadAsync(string bucket, string objectKey, CancellationToken cancellationToken = default);
		Task<bool> ExistsAsync(string bucket, string objectKey, CancellationToken cancellationToken = default);
		Task DeleteAsync(string bucket, string objectKey, CancellationToken cancellationToken = default);
	}
}

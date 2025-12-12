namespace Drive.Application.Contracts
{
	public sealed record StoredFileDownloadDto(
		Stream Stream,
		string ContentType,
		string FileName
	);

	public sealed record StoredFileMetadataDto(
		Guid? FileId,
		string? ObjectKey,
		string? FileName
	);
}

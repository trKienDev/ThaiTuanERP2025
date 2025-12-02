namespace ThaiTuanERP2025.Application.Files.Contracts
{
	public sealed record StoredFileDownloadDto (
		Stream Stream,
		string ContentType,
		string FileName,
		bool IsPublic
	);

	public sealed record StoredFileMetadataDto(
		Guid? FileId,
		string? ObjectKey,
		string? FileName,
		bool IsPublic
	);
}

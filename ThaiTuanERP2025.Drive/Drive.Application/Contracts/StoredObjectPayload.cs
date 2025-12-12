namespace Drive.Application.Contracts
{
	public sealed record StoredObjectPayload
	(
		string Bucket,
		string ObjectKey,
		string FileName,
		string ContentType,
		long Size
	);
}

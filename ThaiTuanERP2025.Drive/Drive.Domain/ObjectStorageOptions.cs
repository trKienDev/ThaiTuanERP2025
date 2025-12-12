namespace Drive.Domain
{
	public sealed class ObjectStorageOptions
	{
		public const string SectionName = "ObjectStorage";
		public required string BasePath { get; set; } = default!;
		public int PresignedExpirySeconds { get; set; } = 300;
		public string PublicRequestPath { get; init; } = "/files/public";
	}
}

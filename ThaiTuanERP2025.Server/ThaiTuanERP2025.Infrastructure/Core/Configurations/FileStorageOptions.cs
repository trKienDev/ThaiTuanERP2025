namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public sealed class FileStorageOptions
	{
		public const string SectionName = "FileStorage";
		public required string BasePath { get; set; } = default!;
		public int PresignedExpirySeconds { get; set; } = 300;
		public string PublicRequestPath { get; init; } = "/files/public";
	}
}

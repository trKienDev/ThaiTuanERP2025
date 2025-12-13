namespace ThaiTuanERP2025.Application.Drive.Contracts
{
	public sealed record DriveObjectDto
	{
		public Guid Id { get; init; }
		public string FileName { get; init; } = null!;
		public string ContentType { get; init; } = null!;
		public long Size { get; init; }

		// Drive trả sẵn URL (presigned hoặc proxy)
		public string DownloadUrl { get; init; } = null!;
	}
}

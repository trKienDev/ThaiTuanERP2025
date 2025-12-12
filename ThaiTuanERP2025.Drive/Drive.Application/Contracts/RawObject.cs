namespace Drive.Application.Contracts
{
	public sealed record RawObject(string FileName, string? ContentType, long Length,
		Func<CancellationToken, Task<Stream>> OpenReadStream // ko phụ thuộc AP.NET Core HTTP --> dễ mock test về sau
	);
}

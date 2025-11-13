namespace ThaiTuanERP2025.Application.Shared.Services
{
	public interface ICodeGenerator
	{
		Task<string> NextAsync(string key, string prefix, int padLength = 6, long start = 1, CancellationToken cancellationToken = default);
	}
}

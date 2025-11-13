namespace ThaiTuanERP2025.Infrastructure.Shared.Querying
{
	public static class KeywordFilter
	{	
		public static string Normalize(string? keyword) =>( keyword ?? string.Empty).Trim();
	}
}

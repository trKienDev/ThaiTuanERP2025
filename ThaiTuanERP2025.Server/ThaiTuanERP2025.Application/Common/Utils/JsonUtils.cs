using System.Text.Json;

namespace ThaiTuanERP2025.Application.Common.Utils
{
	public static class JsonUtils
	{
		public static Guid[] ParseGuidArray(string? json)
		{
			if (string.IsNullOrWhiteSpace(json)) return Array.Empty<Guid>();
			try
			{
				using var doc = JsonDocument.Parse(json);
				return doc.RootElement.EnumerateArray().Select(e => Guid.TryParse(e.GetString(), out var g) ? g : Guid.Empty)
					.Where(g => g != Guid.Empty)
					.ToArray();
			}
			catch { 
				return Array.Empty<Guid>();
			}
		}

		public static string ToJsonArray(IEnumerable<Guid> ids)
		{
			return JsonSerializer.Serialize(ids);
		}
	}
}
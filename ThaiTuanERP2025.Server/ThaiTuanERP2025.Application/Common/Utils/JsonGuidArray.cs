using System.Text.Json;

namespace ThaiTuanERP2025.Application.Common.Utils
{
	public static class JsonGuidArray
	{
		public static Guid[] Parse(string? json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return Array.Empty<Guid>();
			try
			{
				var raw = JsonSerializer.Deserialize<string[]>(json);
				return raw?.Select(Guid.Parse).ToArray() ?? Array.Empty<Guid>();
			}
			catch
			{
				return Array.Empty<Guid>();
			}
		}

		public static string Serialize(IEnumerable<Guid> ids)
		{
			return JsonSerializer.Serialize(ids.Select(x => x.ToString("D")).ToArray());
		}
	}
}

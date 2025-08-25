using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Common.Utils
{
	public static class CodeGenerator
	{
		public static string FromName(string? name, int maxLength = 50) {
			if (string.IsNullOrWhiteSpace(name))
			{
				return string.Empty;
			}
			var processed = RemoveDiacritics(name.Trim());

			// replace non - alphanum by _
			processed = Regex.Replace(processed, @"[^a-zA-Z0-9]+", "_");

			// remove leading/trailing _
			processed = processed.Trim('_').ToLowerInvariant();

			// limit length
			return processed.Length > maxLength ? processed.Substring(0, maxLength) : processed;
		}

		private static string RemoveDiacritics(string text)
		{
			text = text.Replace('đ', 'd').Replace('ê', 'e').Replace('â', 'a').Replace('ă', 'a').Replace('ơ', 'o').Replace('ô', 'o').Replace('ư', 'u');

			var normalized = text.Normalize(NormalizationForm.FormD);
			var builder = new StringBuilder();

			foreach (var ch in normalized)
			{
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark)
					builder.Append(ch);
			}

			return builder.ToString().Normalize(NormalizationForm.FormD);
		}
	}
}

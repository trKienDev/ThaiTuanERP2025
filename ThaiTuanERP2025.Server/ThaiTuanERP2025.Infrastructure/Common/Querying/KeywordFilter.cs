using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Infrastructure.Common.Querying
{
	public static class KeywordFilter
	{	
		public static string Normalize(string? keyword) =>( keyword ?? string.Empty).Trim();
	}
}

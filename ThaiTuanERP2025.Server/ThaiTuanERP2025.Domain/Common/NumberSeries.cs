using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Common
{
	public class NumberSeries : BaseEntity
	{
		// Khóa tự nhiên (duy nhất)
		public string Key { get; set; } = string.Empty;
		public string Prefix { get; set; } = string.Empty;
		public int PadLength { get; set; } = 6;
		public long NextNumber { get; set; } = 1;

		public byte[] RowVersion { get; set; } = default!; // Concurrency token để tránh đụng độ khi nhiều request cùng lúc
	}
}

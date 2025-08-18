using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class Tax :AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public decimal Rate { get; set; } 
		public PriceMode PriceMode { get; set; } = PriceMode.Net; // Mặc định là Net
		public bool IsActive { get; set; } = true; // Mặc định là true
	}
}

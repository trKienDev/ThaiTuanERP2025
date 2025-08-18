using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Enums;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class WithholdingTaxType : AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public decimal Rate { get; set; }
		public WhtBasis Basis { get; set; } = WhtBasis.Gross; // Mặc định là Gross

		public string? GLMapping { get; set; } // Mã tài khoản kế toán liên kết, có thể null nếu không có
		public bool IsActive { get; set; } = true; 
	}
}

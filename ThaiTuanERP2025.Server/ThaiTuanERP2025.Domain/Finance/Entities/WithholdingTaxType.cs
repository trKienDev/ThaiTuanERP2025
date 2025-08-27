using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class WithholdingTaxType : AuditableEntity
	{
		public string Name { get; set; } = default!; // WHT 10%
		public decimal Rate { get; set; } // %
		public string? Description { get; set; }	
		public bool IsActive { get; set; } = true;
	}
}

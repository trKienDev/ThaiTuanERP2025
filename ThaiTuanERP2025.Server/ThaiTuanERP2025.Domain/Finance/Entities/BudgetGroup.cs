using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetGroup : AuditableEntity
	{
		public string Code	{ get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;

		public ICollection<BudgetCode> BudgetCodes { get; set; } = new List<BudgetCode>();

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }
	}
}

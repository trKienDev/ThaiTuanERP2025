using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class CashoutGroup : AuditableEntity
	{
		public string Code { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public bool IsActive { get; set; } = true;

		// self-reference
		public Guid? ParentId { get; set; }
		public CashoutGroup? Parent { get; set; }
		public ICollection<CashoutGroup> Children { get; set; } = new List<CashoutGroup>();

		public ICollection<CashoutCode> CashoutCodes { get; set; } = new List<CashoutCode>();

		// path
		public int Level { get; set; }	// 0 cho root, +1 mỗi tầng
		public string? Path { get; set; }
	}
}

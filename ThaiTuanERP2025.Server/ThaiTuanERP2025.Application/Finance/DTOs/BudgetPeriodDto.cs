using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public class BudgetPeriodDto
	{
		public Guid Id { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public bool IsActive { get; set; } = true;
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	}
}

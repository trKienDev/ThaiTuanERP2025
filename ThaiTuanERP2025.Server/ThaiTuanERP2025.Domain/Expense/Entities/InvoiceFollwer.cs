using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class InvoiceFollwer
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid InvoiceId {get; set; }	
		public Guid UserId { get; set; }

		// navigation
		public Invoice Invoice { get; set; } = default!;	
		public User User { get; set; } = default!;	
	}
}

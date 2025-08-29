using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class Supplier : AuditableEntity
	{
		public string Name { get; private set; } = null!;
		public string? TaxCode { get; private set; }
		public bool IsActive { get; private set; } = true;

		public ICollection<BankAccount> BankAccounts { get; private set; }
		
		private Supplier() {
			BankAccounts = new List<BankAccount>();
		}

		public Supplier(string name, string? taxCode) {
			if (string.IsNullOrWhiteSpace(name)) 
				throw new ValidationException("Supplier", "Tên nhà cung cấp không được để trống");

			Id = Guid.NewGuid();
			Name = name.Trim();
			TaxCode = string.IsNullOrWhiteSpace(taxCode) ? null : taxCode.Trim();
			IsActive = true;
			BankAccounts = new List<BankAccount>();
		}

		public void Activate() => IsActive = true;
		public void Deactive() => IsActive = false;
		public void Rename(string name) {
			if (string.IsNullOrWhiteSpace(name))
				throw new ValidationException("Supplier", "Tên nhà cung cấp không được để trống");
				Name = name.Trim();
		}
		public void SetTaxCode(string? taxCode) => TaxCode = string.IsNullOrWhiteSpace(taxCode) ? null : taxCode.Trim();

	}
}

using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class OutgoingBankAccount : AuditableEntity
	{
		private OutgoingBankAccount() { } // EF
		public OutgoingBankAccount(string name, string bankName, string accountNumber, string ownerName)
		{
			Id = Guid.NewGuid();
			Name = name;
			BankName = bankName.Trim();
			AccountNumber = accountNumber.Trim();
			OwnerName = ownerName.Trim();
		}

		public string Name { get; set; } = null!;
		public string BankName { get; set; } = null!;
		public string AccountNumber { get; set; } = null!;
		public string OwnerName { get; set; } = null!; // Tên chủ tài khoản
		public bool IsActive { get; set; } = true;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public void setName(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên không được để trống");
			Name = name.Trim();
		}

		public void SetOutgoingBankAccount(string bankName, string accountNumber, string ownerName)
		{
			BankName = bankName.Trim();
			AccountNumber = accountNumber.Trim();
			OwnerName = ownerName.Trim();
		}

		public void Deactivate()
		{
			IsActive = false;
		}
		public void Activate()
		{
			IsActive = true;
		}	
	}
}

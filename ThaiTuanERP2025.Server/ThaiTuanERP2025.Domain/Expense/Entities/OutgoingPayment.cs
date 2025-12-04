using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class OutgoingPayment : AuditableEntity
	{
		private readonly List<Guid> _followerIds = new();
		private readonly List<User> _followers = new();

		#region Constructors
		private OutgoingPayment() { } 
		public OutgoingPayment(string name, decimal outgoingAmount, string bankName, string accountNumber, string beneficiaryName, DateTime dueAt, Guid outgoingBankAccountId, Guid expensePaymentId, string? description = null)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstZeroOrNegative(outgoingAmount, nameof(outgoingAmount));
			Guard.AgainstDefault(outgoingBankAccountId, nameof(outgoingBankAccountId));
			Guard.AgainstDefault(expensePaymentId, nameof(expensePaymentId));

			Id = Guid.NewGuid();
			Name = name.Trim();
			Description = description?.Trim() ?? string.Empty;
			BankName = bankName?.Trim() ?? string.Empty;
			AccountNumber = accountNumber?.Trim() ?? string.Empty;
			BeneficiaryName = beneficiaryName?.Trim() ?? string.Empty;
			OutgoingAmount = outgoingAmount;
			DueAt = dueAt;
			OutgoingBankAccountId = outgoingBankAccountId;
			ExpensePaymentId = expensePaymentId;
			Status = OutgoingPaymentStatus.Pending;
			PostingAt = DateTime.UtcNow;
		}
		#endregion

		#region Properties
		public string Name { get; private set; } = string.Empty;
		public string SubId { get; private set; } = default!;
		public string Description { get; private set; } = string.Empty;
		public decimal OutgoingAmount { get; private set; }
		public OutgoingPaymentStatus Status { get; private set; }

		public string BankName { get; private set; } = string.Empty;
		public string AccountNumber { get; private set; } = string.Empty;
		public string BeneficiaryName { get; private set; } = string.Empty;

		public DateTime PostingAt { get; private set; }
		public DateTime PaymentAt { get; private set; }
		public DateTime DueAt { get; private set; }

		public Guid OutgoingBankAccountId { get; private set; }
		public OutgoingBankAccount OutgoingBankAccount { get; private set; } = null!;

		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set; } = null!;

		public Guid? SupplierId { get; set; }
		public Supplier? Supplier { get; set; }
		public Guid? EmployeeId { get; set; }
		public User? Employee { get; set; }

		public IReadOnlyCollection<Guid> FollowerIds => _followerIds.AsReadOnly();
		public IReadOnlyCollection<User> Followers => _followers.AsReadOnly();
		#endregion

		#region Domain Behaviors
		internal void SetSubId(string id)
		{
			Guard.AgainstNullOrWhiteSpace(id, nameof(id));
			SubId = id.Trim();
		}

		internal void AssignSupplier(Guid supplierId)
		{
			if (SupplierId is not null)
				throw new DomainException("Khoản chi này đã được gán với nhà cung cấp");

			if(EmployeeId is not null)
				throw new DomainException("Khoản chi này đã được gán với nhân viên thụ hưởng");

			SupplierId = supplierId;
		}

		internal void UpdateDescription(string? description)
		{
			Description = description?.Trim() ?? string.Empty;
		}

		internal void SetOutgoingAmount(decimal amount)
		{
			Guard.AgainstZeroOrNegative(amount, nameof(amount));
			OutgoingAmount = amount;
		}

		internal void AddFollower(Guid userId)
		{
			if (userId == Guid.Empty) return;
			if (_followerIds.Contains(userId)) return;
			_followerIds.Add(userId);
		}

		internal void RemoveFollower(Guid userId)
		{
			_followerIds.Remove(userId);
		}

		internal void ChangeStatus(OutgoingPaymentStatus newStatus)
		{
			if ((int)newStatus < (int)Status)
				throw new InvalidOperationException($"Không thể chuyển trạng thái từ {Status} về {newStatus}");

			Status = newStatus;
		}

		internal void Approve(Guid actorUserId)
		{
			Guard.AgainstDefault(actorUserId, nameof(actorUserId));

			if (Status != OutgoingPaymentStatus.Pending)
				throw new InvalidOperationException("Chỉ chứng từ ở trạng thái Pending mới được duyệt.");

			ChangeStatus(OutgoingPaymentStatus.Approved);
			PostingAt = DateTime.UtcNow;
		}

		internal void MarkCreated(Guid actorUserId)
		{
			Guard.AgainstDefault(actorUserId, nameof(actorUserId));

			if (Status != OutgoingPaymentStatus.Approved)
				throw new InvalidOperationException("Chỉ chứng từ đã duyệt mới được tạo lệnh.");

			ChangeStatus(OutgoingPaymentStatus.Created);
			PaymentAt = DateTime.UtcNow;
		}

		internal void Cancel(Guid actorUserId)
		{
			Guard.AgainstDefault(actorUserId, nameof(actorUserId));

			if (Status == OutgoingPaymentStatus.Cancelled)
				throw new InvalidOperationException("Chứng từ đã bị hủy.");

			ChangeStatus(OutgoingPaymentStatus.Cancelled);
		}


		#endregion
	}
}

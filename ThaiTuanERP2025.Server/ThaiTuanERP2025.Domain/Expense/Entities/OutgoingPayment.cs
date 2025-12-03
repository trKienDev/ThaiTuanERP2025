using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Account.Events;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Expense.Events.OutgoingPayments;

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

		public Guid? SupplierId { get; private set; }
		public Supplier? Supplier { get; private set; }
		public Guid? EmployeeId { get; private set; }
		public User? Employee { get; private set; }

		public IReadOnlyCollection<Guid> FollowerIds => _followerIds.AsReadOnly();
		public IReadOnlyCollection<User> Followers => _followers.AsReadOnly();
		#endregion

		#region Domain Behaviors
		public void SetSubId(string id)
		{
			Guard.AgainstNullOrWhiteSpace(id, nameof(id));
			SubId = id.Trim();
		}

		public void UpdateDescription(string? description)
		{
			Description = description?.Trim() ?? string.Empty;
		}

		public void SetOutgoingAmount(decimal amount)
		{
			Guard.AgainstZeroOrNegative(amount, nameof(amount));
			OutgoingAmount = amount;
		}

		public void AddFollower(Guid userId)
		{
			if (userId == Guid.Empty) return;
			if (_followerIds.Contains(userId)) return;
			_followerIds.Add(userId);
		}

		public void RemoveFollower(Guid userId)
		{
			_followerIds.Remove(userId);
		}

		public void ChangeStatus(OutgoingPaymentStatus newStatus)
		{
			if ((int)newStatus < (int)Status)
				throw new InvalidOperationException($"Không thể chuyển trạng thái từ {Status} về {newStatus}");

			Status = newStatus;
			AddDomainEvent(new OutgoingPaymentStatusChangedEvent(this, newStatus));
		}

		public void Approve(Guid actorUserId)
		{
			Guard.AgainstDefault(actorUserId, nameof(actorUserId));

			if (Status != OutgoingPaymentStatus.Pending)
				throw new InvalidOperationException("Chỉ chứng từ ở trạng thái Pending mới được duyệt.");

			ChangeStatus(OutgoingPaymentStatus.Approved);
			PostingAt = DateTime.UtcNow;
			AddDomainEvent(new OutgoingPaymentApprovedEvent(this, actorUserId));
		}

		public void MarkCreated(Guid actorUserId)
		{
			Guard.AgainstDefault(actorUserId, nameof(actorUserId));

			if (Status != OutgoingPaymentStatus.Approved)
				throw new InvalidOperationException("Chỉ chứng từ đã duyệt mới được tạo lệnh.");

			ChangeStatus(OutgoingPaymentStatus.Created);
			PaymentAt = DateTime.UtcNow;
			AddDomainEvent(new OutgoingPaymentCreatedDomainEvent(this, actorUserId));
		}

		public void Cancel(Guid actorUserId)
		{
			Guard.AgainstDefault(actorUserId, nameof(actorUserId));

			if (Status == OutgoingPaymentStatus.Cancelled)
				throw new InvalidOperationException("Chứng từ đã bị hủy.");

			ChangeStatus(OutgoingPaymentStatus.Cancelled);
			AddDomainEvent(new OutgoingPaymentCancelledEvent(this, actorUserId));
		}
		#endregion
	}
}

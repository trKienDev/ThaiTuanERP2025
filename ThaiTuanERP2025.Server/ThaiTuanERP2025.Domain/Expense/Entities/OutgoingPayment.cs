using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class OutgoingPayment : AuditableEntity
	{
		// === Backing fields ===
		private readonly List<Guid> _followerIds = new();
		private readonly List<User> _followers = new();

		// === EF Constructor ===
		private OutgoingPayment() {}

		// === Domain Constructor ===
		public OutgoingPayment(
			string name, decimal outgoingAmount,
			string bankName, string accountNumber, string beneficiaryName, 
			DateTime dueDate,
			Guid outgoingBankAccountId, Guid expensePaymentId,
			string? description = null
		) {
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Tên chứng từ không được để trống.", nameof(name));
			if (outgoingAmount <= 0)
				throw new ArgumentException("Số tiền chi phải lớn hơn 0.", nameof(outgoingAmount));

			Id = Guid.NewGuid();
			Name = name.Trim();
			Description = description?.Trim() ?? string.Empty;
			BankName = bankName?.Trim() ?? string.Empty;
			AccountNumber = accountNumber?.Trim() ?? string.Empty;
			BeneficiaryName = beneficiaryName?.Trim() ?? string.Empty;
			OutgoingAmount = outgoingAmount;
			DueDate = dueDate;
			OutgoingBankAccountId = outgoingBankAccountId;
			ExpensePaymentId = expensePaymentId;
			Status = OutgoingPaymentStatus.Pending;
		}


		public string Name { get; private set; } = string.Empty;
		public string SubId { get; private set; } = default!;
		public string Description { get; private set; } = string.Empty;
		public decimal OutgoingAmount { get; private set; }
		public OutgoingPaymentStatus Status { get; private set; }

		public PayeeType PayeeType { get; private set; }
		public Guid? SupplierId { get; private set; }
		public Supplier? Supplier { get; private set; }
		public Guid? EmployeeId { get; private set; }
		public User? Employee { get; private set; }

		public string BankName { get; private set; } = string.Empty;
		public string AccountNumber { get; private set; } = string.Empty;
		public string BeneficiaryName { get; private set; } = string.Empty;

		public DateTime PostingDate { get; private set; } // Ngày ghi nhận khi duyệt
		public DateTime PaymentDate { get; private set; } // ngày tạo lệnh
		public DateTime DueDate { get; private set; } // hạn thanh toán

		public Guid OutgoingBankAccountId { get; private set; }
		public OutgoingBankAccount OutgoingBankAccount { get; private set; } = null!;

		public Guid ExpensePaymentId { get; private set; }
		public ExpensePayment ExpensePayment { get; private set;} = null!;

		public IReadOnlyCollection<Guid> FollowerIds => _followerIds.AsReadOnly();
		public IReadOnlyCollection<User> Followers => _followers.AsReadOnly();

		// Auditable
		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public void SetSubId(string id) {
			if (string.IsNullOrWhiteSpace(id))
				throw new ArgumentException("SubId is required");
			SubId = id;
 		}

		public void SetSupplierId(Guid? supplierId)
		{
			SupplierId = supplierId;
		}
		
		public void SetEmployeeId(Guid? employeeId) {
			EmployeeId = employeeId;
		}

		public void UpdateDescription(string? description)
			=> Description = description?.Trim() ?? string.Empty;

		public void SetOutgoingAmount(decimal amount)
		{
			if (amount <= 0)
				throw new ArgumentException("Số tiền chi phải lớn hơn 0.");
			OutgoingAmount = amount;
		}

		public void SetPaymentDate(DateTime paymentDate)
		{
			PaymentDate = paymentDate;
		}

		public void LinkExpensePayment(Guid expensePaymentId)
		{
			if (expensePaymentId == Guid.Empty)
				throw new ArgumentException("ExpensePaymentId không hợp lệ.");
			ExpensePaymentId = expensePaymentId;
		}

		public void LinkOutgoingBankAccount(Guid outgoingBankAccountId)
		{
			if (outgoingBankAccountId == Guid.Empty)
				throw new ArgumentException("OutgoingBankAccountId không hợp lệ.");
			OutgoingBankAccountId = outgoingBankAccountId;
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

		public void ReplaceFollowers(IEnumerable<Guid> userIds)
		{
			var set = new HashSet<Guid>(userIds.Where(x => x != Guid.Empty));
			_followerIds.Clear();
			_followerIds.AddRange(set);
		}

		public void ChangeStatus(OutgoingPaymentStatus newStatus)
		{
			// 🔸 Domain rule: không thể quay lại trạng thái trước
			if ((int)newStatus < (int)Status)
				throw new InvalidOperationException($"Không thể chuyển trạng thái từ {Status} về {newStatus}");

			Status = newStatus;
		}

		public void Approve(Guid actorUserId) {
			if (actorUserId == Guid.Empty)
				throw new ArgumentException("Thiếu người thực hiện.", nameof(actorUserId));

			if (Status != OutgoingPaymentStatus.Pending)
				throw new InvalidOperationException("Chỉ chứng từ ở trạng thái Pending mới được duyệt.");

			if (actorUserId != CreatedByUserId)
				throw new InvalidOperationException("Chỉ người tạo chứng từ mới có quyền duyệt.");

			ChangeStatus(OutgoingPaymentStatus.Approved);
			PostingDate = DateTime.UtcNow;
		}

		public void MarkCreated(Guid actorUserId)
		{
			if (actorUserId == Guid.Empty)
				throw new ArgumentException("Thiếu người thực hiện.", nameof(actorUserId));

			if (Status != OutgoingPaymentStatus.Approved)
				throw new InvalidOperationException("Chỉ chứng từ ở trạng thái duyệt mới được tạo lệnh.");

			if (actorUserId != CreatedByUserId)
				throw new InvalidOperationException("Chỉ người tạo chứng từ mới có quyền duyệt.");

			ChangeStatus(OutgoingPaymentStatus.Created);
			PaymentDate = DateTime.UtcNow;
		}

		public void Cancel(Guid actorUserId)
		{
			if (actorUserId == Guid.Empty)
				throw new ArgumentException("Thiếu người thực hiện.", nameof(actorUserId));
			if (Status == OutgoingPaymentStatus.Cancelled)
				throw new InvalidOperationException("Chứng từ đã ở trạng thái Hủy.");
			if (actorUserId != CreatedByUserId)
				throw new InvalidOperationException("Chỉ người tạo chứng từ mới có quyền hủy.");
			ChangeStatus(OutgoingPaymentStatus.Cancelled);
		}	
	}
}

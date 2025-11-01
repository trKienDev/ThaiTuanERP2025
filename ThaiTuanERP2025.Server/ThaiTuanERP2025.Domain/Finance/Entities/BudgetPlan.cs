using System.ComponentModel.DataAnnotations;
using System.Transactions;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Finance.Events;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPlan : AuditableEntity
	{
		#region Constructors
		private BudgetPlan() { }
		public BudgetPlan (Guid departmentId, Guid budgetCodeId, Guid budgetPeriodId, decimal amount ) {
			if (amount <= 0)
				throw new ArgumentException("Amount must be greater than zero", nameof(amount));

			this.DepartmentId = departmentId;	
			this.BudgetCodeId = budgetCodeId;
			this.BudgetPeriodId = budgetPeriodId;
			this.Amount = amount;
			this.IsActive = true;
		}
		#endregion

		#region Properties
		public Guid DepartmentId { get; private set; }
		public Guid BudgetCodeId { get; private set; }
		public Guid BudgetPeriodId { get; private set; }
		public decimal Amount {  get; private set; }
		public bool IsActive { get; private set; } = true;
		public BudgetPlanStatus Status { get; private set; } = BudgetPlanStatus.Draft;

		[Timestamp]
		public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

		private readonly List<BudgetTransaction> _transactions = new();
		public IReadOnlyCollection<BudgetTransaction> Transactions => _transactions.AsReadOnly();

		public Guid? ReviewedByUserId { get; private set; } 
		public User? ReviewedByUser { get; private set; }
		public DateTime? ReviewedAt { get; private set; }

		public Guid? ApprovedByUserId { get; private set; } 
		public User? ApprovedByUser { get; private set; }
		public DateTime? ApprovedAt { get; private set; }

		public BudgetCode BudgetCode { get; private set; } = null!;
		public BudgetPeriod BudgetPeriod { get; private set; } = null!;
		public Department Department { get; private set; } = null!;
		#endregion


		#region Domain Behaviors
		public void MarkReviewed(Guid userId)
		{
			if (Status != BudgetPlanStatus.Draft)
				throw new InvalidOperationException("Chỉ có thể xem xét kế hoạch ở trạng thái Draft.");

			Status = BudgetPlanStatus.Reviewed;
			ReviewedByUserId = userId;
			ReviewedAt = DateTime.UtcNow;
			AddDomainEvent(new BudgetPlanReviewedEvent(Id, userId));
		}

		public void Approve(Guid userId)
		{
			if (Status != BudgetPlanStatus.Reviewed)
				throw new InvalidOperationException("Chỉ có thể phê duyệt kế hoạch đã được xem xét.");

			Status = BudgetPlanStatus.Approved;
			ApprovedByUserId = userId;
			ApprovedAt = DateTime.UtcNow;
			AddDomainEvent(new BudgetPlanApprovedEvent(Id, userId));
		}

		public void Reject(Guid userId)
		{
			if (Status == BudgetPlanStatus.Approved)
				throw new InvalidOperationException("Không thể từ chối kế hoạch đã được phê duyệt.");

			Status = BudgetPlanStatus.Rejected;
			ApprovedByUserId = userId;
			ApprovedAt = DateTime.UtcNow;
			AddDomainEvent(new BudgetPlanRejectedEvent(Id, userId));
		}

		public void Activate() => IsActive = true;
		public void Deactivate() => IsActive = false;

		/// <summary>
		/// Được gọi nội bộ trong aggregate BudgetPeriod để gán liên kết đúng kỳ ngân sách.
		/// </summary>
		internal void AssignToPeriod(Guid periodId)
		{
			BudgetPeriodId = periodId;
		}

		public void SetAmount(decimal amount)
		{
			if (amount <= 0)
				throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

			if (Status == BudgetPlanStatus.Approved)
				throw new InvalidOperationException("Không thể thay đổi ngân sách sau khi đã được phê duyệt.");

			if (Status == BudgetPlanStatus.Reviewed)
				throw new InvalidOperationException("Không thể thay đổi ngân sách sau khi đã được xem xét.");

			Amount = amount;
		}

		/// <summary>
		/// Kiểm tra ngân sách hiện tại có đủ để chi không.
		/// </summary>
		public bool CanAfford(decimal requiredAmount)
		{
			return requiredAmount > 0 && Amount >= requiredAmount;
		}

		/// <summary>
		/// Trừ tiền từ ngân sách (đã được phê duyệt).
		/// </summary>
		public void RecordPayment(decimal amount, Guid recordId)
		{
			if (amount <= 0)
				throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

			if (Status != BudgetPlanStatus.Approved)
				throw new InvalidOperationException("Chỉ có thể ghi nhận chi tiêu cho kế hoạch đã được phê duyệt.");

			if (Amount < amount)
				throw new InvalidOperationException($"Ngân sách không đủ. Còn lại {Amount:n0}, cần {amount:n0}.");

			Amount -= amount;

			var transaction = new BudgetTransaction(Id, recordId, amount, BudgetTransactionType.ExpensePayment);
			_transactions.Add(transaction);

			AddDomainEvent(new BudgetPlanTransactionRecordedEvent(Id, transaction.Id, amount, BudgetTransactionType.ExpensePayment));
		}

		/// Hoàn lại (cộng lại) ngân sách khi có điều chỉnh hoặc hoàn tiền.
		public void RefundPayment(decimal amount, Guid refundId)
		{
			if (amount <= 0)
				throw new ArgumentException("Số tiền hoàn phải lớn hơn 0.", nameof(amount));

			Amount += amount;

			_transactions.Add(new BudgetTransaction(
				budgetPlanId: Id,
				paymentId: refundId,
				amount: amount,
				type: BudgetTransactionType.Refund
			));
		}

		/// Kiểm tra còn ngân sách khả dụng hay không.
		public bool HasRemainingBudget() => Amount > 0;
		#endregion

	}
}

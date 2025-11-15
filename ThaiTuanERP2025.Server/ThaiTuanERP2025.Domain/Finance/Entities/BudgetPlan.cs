using System.ComponentModel.DataAnnotations;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Finance.Events;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPlan : AuditableEntity
	{
		#region Constructors
		private BudgetPlan() { }
		public BudgetPlan(Guid departmentId, Guid budgetPeriodId,  Guid reviewerId, Guid approverId)
		{
			this.DepartmentId = departmentId;
			this.BudgetPeriodId = budgetPeriodId;
			SelectedReviewerId = reviewerId;
			SelectedApproverId = approverId;

			Status = BudgetPlanStatus.Draft;
			this.IsActive = true;

			DueAt = DateTime.UtcNow.AddHours(8);
			AddDomainEvent(new BudgetPlanCreatedEvent(this, reviewerId, DueAt.Value));
		}
		#endregion

		private readonly List<BudgetPlanDetail> _details = new();
		#region Properties
		public Guid DepartmentId { get; private set; }
		public Guid BudgetPeriodId { get; private set; }
		public decimal TotalAmount  { get; private set; }
		public bool IsActive { get; private set; } = true;
		public BudgetPlanStatus Status { get; private set; } = BudgetPlanStatus.Draft;
		public DateTime? DueAt { get; private set; }
		[Timestamp]
		public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

		//private readonly List<BudgetTransaction> _transactions = new();
		//public IReadOnlyCollection<BudgetTransaction> Transactions => _transactions.AsReadOnly();

		public bool IsReviewed { get; private set; } = false;
		public Guid SelectedReviewerId { get; private set; }
		public Guid? ReviewedByUserId { get; private set; }
		public User? ReviewedByUser { get; init; }
		public DateTime? ReviewedAt { get; private set; }

		public bool IsApproved { get; private set; } = false;
		public Guid SelectedApproverId { get; private set; }
		public BudgetApprover? BudgetApprover { get; init; }
		public Guid? ApprovedByUserId { get; private set; }
		public User? ApprovedByUser { get; init; }
		public DateTime? ApprovedAt { get; private set; }

		public BudgetCode BudgetCode { get; private set; } = null!;
		public BudgetPeriod BudgetPeriod { get; private set; } = null!;
		public Department Department { get; private set; } = null!;
		public IReadOnlyCollection<BudgetPlanDetail> Details => _details.AsReadOnly();
		#endregion

		#region Domain Behaviors
		internal void AssignToPeriod(Guid periodId)
		{
			BudgetPeriodId = periodId;
		}
		public void MarkReviewed(Guid userId)
		{
			if (Status != BudgetPlanStatus.Draft)
				throw new InvalidOperationException("Chỉ có thể xem xét kế hoạch ở trạng thái Draft.");

			Status = BudgetPlanStatus.Reviewed;
			ReviewedByUserId = userId;
			ReviewedAt = DateTime.UtcNow;
			AddDomainEvent(new BudgetPlanReviewedEvent(Id, userId));
		}

		public void Approve(Guid approverId)
		{
			Guard.AgainstDefault(approverId, nameof(approverId));

			if (Status != BudgetPlanStatus.Reviewed)
				throw new InvalidOperationException("Kế hoạch ngân sách chưa được xem xét.");

			ApprovedByUserId = approverId;
			ApprovedAt = DateTime.UtcNow;
			Status = BudgetPlanStatus.Approved;

			AddDomainEvent(new BudgetPlanApprovedEvent(Id, approverId));
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


		public void MoveToApproval(BudgetApprover budgetApprover)
		{
			Guard.AgainstNull(budgetApprover, nameof(budgetApprover));

			ApprovedByUserId = budgetApprover.ApproverUserId;
			SelectedApproverId = budgetApprover.Id;
			DueAt = DateTime.UtcNow.AddHours(budgetApprover.SlaHours);
			Status = BudgetPlanStatus.Reviewed;

			AddDomainEvent(new BudgetPlanAssignedForApprovalEvent(Id, ApprovedByUserId.Value, DueAt.Value));
		}

		public BudgetPlanDetail AddDetail(Guid budgetCodeId, decimal amount, Guid userId)
		{
			if (Status != BudgetPlanStatus.Draft)
				throw new DomainException("Chỉ được thêm chi tiết khi kế hoạch đang ở trạng thái Draft.");

			if (_details.Any(d => d.BudgetCodeId == budgetCodeId && d.IsActive))
				throw new DomainException("Mã ngân sách đã tồn tại trong kế hoạch này.");

			var detail = new BudgetPlanDetail(Id, budgetCodeId, amount, userId);
			_details.Add(detail);

			// AddDomainEvent(new BudgetPlanDetailAddedEvent(Id, budgetCodeId, amount));

			return detail;
		}

		public void UpdateDetail(Guid budgetCodeId, decimal newAmount, Guid userId)
		{
			var detail = _details.FirstOrDefault(d => d.BudgetCodeId == budgetCodeId && d.IsActive)
			    ?? throw new DomainException("Không tìm thấy chi tiết ngân sách.");

			detail.UpdateAmount(newAmount, userId);

			// AddDomainEvent(new BudgetPlanDetailUpdatedEvent(Id, budgetCodeId, newAmount));
		}

		public void RemoveDetail(Guid budgetCodeId, Guid userId)
		{
			var detail = _details.FirstOrDefault(d => d.BudgetCodeId == budgetCodeId && d.IsActive)
			    ?? throw new DomainException("Không tìm thấy chi tiết ngân sách.");

			detail.SoftDelete(userId);

			// AddDomainEvent(new BudgetPlanDetailRemovedEvent(Id, budgetCodeId));
		}
		#endregion
	}
}

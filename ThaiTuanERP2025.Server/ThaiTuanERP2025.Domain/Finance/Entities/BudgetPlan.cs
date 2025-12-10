using System.ComponentModel.DataAnnotations;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Domain.Finance.Events;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Shared.Interfaces;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetPlan : AuditableEntity, IActiveEntity
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
			AddDomainEvent(new BudgetPlanCreatedEvent(this, reviewerId, DueAt));
		}
		#endregion

		#region Properties
		private readonly List<BudgetPlanDetail> _details = new();

		public Guid DepartmentId { get; private set; }
		public Guid BudgetPeriodId { get; private set; }
		public bool IsActive { get; private set; } = true;
		public BudgetPlanStatus Status { get; private set; } = BudgetPlanStatus.Draft;
		public DateTime DueAt { get; private set; }
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

		public BudgetPeriod BudgetPeriod { get; private set; } = null!;
		public Department Department { get; private set; } = null!;
		public IReadOnlyCollection<BudgetPlanDetail> Details => _details.AsReadOnly();
		#endregion

		#region Domain Behaviors
		internal void AssignToPeriod(Guid periodId)
		{
			BudgetPeriodId = periodId;
		}
		internal void MarkReviewed(Guid reviewerId, int slaHours = 8)
		{
			if (Status != BudgetPlanStatus.Draft)
				throw new InvalidOperationException("Chỉ có thể xem xét kế hoạch ở trạng thái Draft.");

			Status = BudgetPlanStatus.Reviewed;
			ReviewedByUserId = reviewerId;
			ReviewedAt = DateTime.UtcNow;
			DueAt = DateTime.UtcNow.AddHours(slaHours);
			AddDomainEvent(new BudgetPlanReviewedEvent(this, this.SelectedApproverId, DueAt));
		}

		internal void Approve(Guid approverId, int slaHours = 8)
		{
			Guard.AgainstDefault(approverId, nameof(approverId));

			if (Status != BudgetPlanStatus.Reviewed)
				throw new InvalidOperationException("Kế hoạch ngân sách chưa được xem xét.");

			ApprovedByUserId = approverId;
			ApprovedAt = DateTime.UtcNow;
			Status = BudgetPlanStatus.Approved;

			AddDomainEvent(new BudgetPlanApprovedEvent(this, approverId));
		}

		internal void Reject(Guid userId)
		{
			if (Status == BudgetPlanStatus.Approved)
				throw new InvalidOperationException("Không thể từ chối kế hoạch đã được phê duyệt.");

			Status = BudgetPlanStatus.Rejected;
			ApprovedByUserId = userId;
			ApprovedAt = DateTime.UtcNow;
			// AddDomainEvent(new BudgetPlanRejectedEvent(Id, userId));
		}

		internal void Activate() => IsActive = true;
		internal void Deactivate() => IsActive = false;

		internal void MoveToApproval(BudgetApprover budgetApprover)
		{
			Guard.AgainstNull(budgetApprover, nameof(budgetApprover));

			ApprovedByUserId = budgetApprover.ApproverUserId;
			SelectedApproverId = budgetApprover.Id;
			DueAt = DateTime.UtcNow.AddHours(budgetApprover.SlaHours);
			Status = BudgetPlanStatus.Reviewed;

			// AddDomainEvent(new BudgetPlanAssignedForApprovalEvent(Id, ApprovedByUserId.Value, DueAt));
		}

		internal BudgetPlanDetail AddDetail(Guid budgetCodeId, decimal amount)
		{
			if (Status != BudgetPlanStatus.Draft)
				throw new DomainException("Chỉ được thêm chi tiết khi kế hoạch đang ở trạng thái Draft.");

			if (_details.Any(d => d.BudgetCodeId == budgetCodeId && d.IsActive))
				throw new DomainException("Mã ngân sách đã tồn tại trong kế hoạch này.");

			var detail = new BudgetPlanDetail(Id, budgetCodeId, amount);
			_details.Add(detail);

			// AddDomainEvent(new BudgetPlanDetailAddedEvent(Id, budgetCodeId, amount));

			return detail;
		}

		internal void UpdateDetail(Guid budgetCodeId, decimal newAmount, Guid userId)
		{
			var detail = _details.FirstOrDefault(d => d.BudgetCodeId == budgetCodeId && d.IsActive)
			    ?? throw new DomainException("Không tìm thấy chi tiết ngân sách.");

			detail.UpdateAmount(newAmount, userId);

			// AddDomainEvent(new BudgetPlanDetailUpdatedEvent(Id, budgetCodeId, newAmount));
		}

		internal void RemoveDetail(Guid budgetCodeId, Guid userId)
		{
			var detail = _details.FirstOrDefault(d => d.BudgetCodeId == budgetCodeId && d.IsActive)
			    ?? throw new DomainException("Không tìm thấy chi tiết ngân sách.");

			detail.SoftDelete(userId);

			// AddDomainEvent(new BudgetPlanDetailRemovedEvent(Id, budgetCodeId));
		}
		#endregion
	}
}

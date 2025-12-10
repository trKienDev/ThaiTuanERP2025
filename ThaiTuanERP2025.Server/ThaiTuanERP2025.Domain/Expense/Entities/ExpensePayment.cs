using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public sealed class ExpensePayment : AuditableEntity
	{
		#region Constructor
		private ExpensePayment() { } // EF
		public ExpensePayment(
			string name, bool hasGoodsReceipt, ExpensePayeeType payeeType, DateTime dueAt, Guid managerApproverId, string? description
		)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstInvalidEnumValue(payeeType, nameof(payeeType));
			Guard.AgainstDefault(managerApproverId, nameof(managerApproverId));

			Id = Guid.NewGuid();
			Name = name.Trim();
			HasGoodsReceipt = hasGoodsReceipt;
			PayeeType = payeeType;
			DueAt = dueAt;
			ManagerApproverId = managerApproverId;
			Status = ExpensePaymentStatus.Pending;
			Description = description?.Trim() ?? string.Empty;
		}
		#endregion

		#region Properties
		private readonly List<ExpensePaymentItem> _items = new();
		private readonly List<OutgoingPayment> _outgoingPayments = new();
		private readonly List<ExpensePaymentAttachment> _attachments = new();

		public string Name { get; private set; } = string.Empty;
		public string SubId { get; private set; } = default!;
		public ExpensePayeeType PayeeType { get; private set; }
		public Guid? SupplierId { get; private set; }
		public Supplier? Supplier { get; set; } 

		public string BankName { get; private set; } = string.Empty;
		public string AccountNumber { get; private set; } = string.Empty;
		public string BeneficiaryName { get; private set; } = string.Empty;

		public DateTime DueAt { get; private set; }
		public bool HasGoodsReceipt { get; private set; }
		public string? Description { get; private set; }

		public decimal TotalAmount { get; private set; }
		public decimal TotalTax { get; private set; }
		public decimal TotalWithTax { get; private set; }
		public decimal OutgoingAmountPaid { get; private set; } = 0;
		public decimal RemainingOutgoingAmount { get; private set; }

		public ExpensePaymentStatus Status { get; private set; }

		public IReadOnlyCollection<ExpensePaymentItem> Items => _items.AsReadOnly();
		public IReadOnlyCollection<OutgoingPayment> OutgoingPayments => _outgoingPayments.AsReadOnly();
                public IReadOnlyCollection<ExpensePaymentAttachment> Attachments => _attachments.AsReadOnly();

                public Guid? CurrentWorkflowInstanceId { get; private set; }
		public ExpenseWorkflowInstance? CurrentWorkflowInstance { get; private set; }
		public Guid ManagerApproverId { get; private set; }

		#endregion

		#region Domain Behaviors
		internal void SetSubId(string subId)
		{
			Guard.AgainstNullOrWhiteSpace(subId, nameof(subId));
			SubId = subId.Trim();
		}

		internal void SetSupplier(Guid? supplierId)
		{
			SupplierId = supplierId;
		}

		internal void SetBankInfo(string bankName, string accountNumber, string beneficiaryName)
		{
			Guard.AgainstNullOrWhiteSpace(bankName, nameof(bankName));
			Guard.AgainstNullOrWhiteSpace(accountNumber, nameof(accountNumber));
			Guard.AgainstNullOrWhiteSpace(beneficiaryName, nameof(beneficiaryName));

			BankName = bankName.Trim();
			AccountNumber = accountNumber.Trim();
			BeneficiaryName = beneficiaryName.Trim();
		}

		internal ExpensePaymentItem AddItem(string itemName,  int quantity, decimal unitPrice, decimal taxRate,  decimal amount, decimal taxAmount, decimal totalWithTax, Guid budgetPlanDetailId, Guid? invoiceFileId = null)
		{
			var item = new ExpensePaymentItem(Id, itemName, quantity, unitPrice, taxRate, amount, taxAmount, totalWithTax, budgetPlanDetailId, invoiceFileId);
			_items.Add(item);
			RecalculateTotals();
			return item;
		}

		// ==== STATUS ====
		internal void Submit()
		{
			if (!_items.Any())
				throw new DomainException("Không thể gửi duyệt phiếu không có hạng mục chi.");
			Status = ExpensePaymentStatus.Submitted;
		}

		internal void MarkApproved()
		{
			if(Status != ExpensePaymentStatus.Pending)
				throw new DomainException("Thanh toán chưa ở trạng thái chờ duyệt");
			Status = ExpensePaymentStatus.Approved;
		}

		internal void MarkRejected()
		{
			if (Status != ExpensePaymentStatus.Pending)
				throw new DomainException("Thanh toán chưa ở trạng thái chờ duyệt");

			Status = ExpensePaymentStatus.Rejected;
		}

		internal void Cancel()
		{
			Status = ExpensePaymentStatus.Cancelled;
		}

		internal void ReadyForOutgoingPayment()
		{
			Status = ExpensePaymentStatus.ReadyForPayment;
		}


		internal void RecalculateTotals()
		{
			var oldTotalWithTax = TotalWithTax;

			// Cộng từng phần
			TotalAmount = _items.Sum(i => i.Amount);
			TotalTax = _items.Sum(i => i.TaxAmount);
			TotalWithTax = _items.Sum(i => i.TotalWithTax);
			RemainingOutgoingAmount = TotalWithTax - OutgoingAmountPaid;

			// Nếu có thay đổi tổng -> raise domain event
			if (TotalWithTax != oldTotalWithTax)
			{
				// AddDomainEvent(new ExpensePaymentTotalsChangedEvent(this));
			}
		}

		internal void LinkWorkflowInstance(ExpenseWorkflowInstance instance)
		{
			Guard.AgainstNull(instance, nameof(instance));
			CurrentWorkflowInstanceId = instance.Id;
			CurrentWorkflowInstance = instance;
		}

		// ==== ATTACHMENTS ====
		internal ExpensePaymentAttachment AddAttachment(Guid storedFileId)
		{
			Guard.AgainstDefault(storedFileId, nameof(storedFileId));

			// Id ở đây là Id của chính ExpensePayment
			var att = new ExpensePaymentAttachment(Id, storedFileId);
			_attachments.Add(att);
			return att;
		}

		internal void AddAttachments(IEnumerable<Guid> storedFileIds)
		{
			foreach (var id in storedFileIds)
				AddAttachment(id);
		}

		// ====  OUTGOING AMOUNT ====
		internal void RegisterOutgoingPayment(decimal outgoingAmount)
		{
			if (outgoingAmount <= 0)
				throw new DomainException("Số tiền phải lớn hơn 0");

			if (outgoingAmount > RemainingOutgoingAmount)
				throw new DomainException("Số tiền khoản chi vượt quá số tiền còn lại của khoản thanh toán");

			OutgoingAmountPaid += outgoingAmount;
			RemainingOutgoingAmount = TotalWithTax - OutgoingAmountPaid;

			if (RemainingOutgoingAmount <= 0)
			{
				RemainingOutgoingAmount = 0;
				Status = ExpensePaymentStatus.FullyPaid;
			}
			else
			{
				Status = ExpensePaymentStatus.PartiallyPaid;
			}
		}
		#endregion
	}
}

using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePayment : AuditableEntity
	{
		private readonly List<ExpensePaymentItem> _items = new();

		private ExpensePayment() { } // EF

		public ExpensePayment(string name, PayeeType payeeType, DateTime dueDate, string managerApproverId, string? description)
		{
			Id = Guid.NewGuid();
			ManagerApproverId = Guid.Parse(managerApproverId);
			Name = name.Trim();
			PayeeType = payeeType;
			DueDate = dueDate;
			Status = ExpensePaymentStatus.Pending;
			Description = description?.Trim() ?? string.Empty;
		}

		// Thông tin chung
		public string Name { get; private set; } = string.Empty;
		public string SubId { get; private set; } = default!;

		// Đối tượng thụ hưởng
		public PayeeType PayeeType { get; private set; }
		public Guid? SupplierId { get; private set; }   // null nếu Employee
		public Supplier? Supplier { get; private set; }  // optional nav

		// Thông tin chuyển khoản (ghi nhận “snapshot” tại thời điểm tạo)
		public string BankName { get; private set; } = string.Empty;
		public string AccountNumber { get; private set; } = string.Empty;
		public string BeneficiaryName { get; private set; } = string.Empty;

		// Lịch thanh toán
		public DateTime DueDate { get; private set; }
		public bool HasGoodsReceipt { get; private set; }
		public string? Description { get; set; } = string.Empty;

		// Tổng cộng (được item cộng dồn)
		public decimal TotalAmount { get; private set; }     // 18,2
		public decimal TotalTax { get; private set; }        // 18,2
		public decimal TotalWithTax { get; private set; }    // 18,2

		// Tổng số tiền khoản chi đã tạo lệnh
		public decimal OutgoingAmountPaid { get; private set; } = 0;
		public decimal RemainingOutgoingAmount { get; private set; }


		// Trạng thái luồng duyệt/chi
		public ExpensePaymentStatus Status { get; private set; }

		// Dòng hạng mục
		public IReadOnlyCollection<ExpensePaymentItem> Items => _items;

		// Đính kèm (tùy dự án, có thể dùng bảng chung Attachment; ở đây minh họa entity riêng)
		private readonly List<ExpensePaymentAttachment> _attachments = new();
		public IReadOnlyCollection<ExpensePaymentAttachment> Attachments => _attachments;

		// Workflow instance id (nếu có)
		public Guid? CurrentWorkflowInstanceId { get; private set; }
		public ApprovalWorkflowInstance? CurrentWorkflowInstance { get; private set; }

		public Guid ManagerApproverId { get; private set; }

		private readonly List<OutgoingPayment> _outgoingPayments = new();
		public IReadOnlyCollection<OutgoingPayment> OutgoingPayments => _outgoingPayments.AsReadOnly();

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		// ==== HÀM NGHIỆP VỤ ====
		public void SetSubId(string value)
		{
			if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("SubId is required");
			SubId = value;
		}

		public void SetSupplier(Guid? supplierId)
		{
			SupplierId = supplierId;
		}

		public void SetBankInfo(string bankName, string accountNumber, string beneficiaryName)
		{
			BankName = bankName?.Trim() ?? string.Empty;
			AccountNumber = accountNumber?.Trim() ?? string.Empty;
			BeneficiaryName = beneficiaryName?.Trim() ?? string.Empty;
		}

		public void SetDueDate(DateTime date) => DueDate = date;

		public void SetGoodsReceipt(bool hasGoodsReceipt) => HasGoodsReceipt = hasGoodsReceipt;

		public ExpensePaymentItem AddItem(string itemName, int quantity, decimal unitPrice,
						  decimal taxRate, Guid? budgetCodeId, Guid? cashoutCodeId,
						  Guid? invoiceId = null, decimal? overrideTaxAmount = null)
		{
			var item = new ExpensePaymentItem(Id, itemName, quantity, unitPrice, taxRate, budgetCodeId, cashoutCodeId, invoiceId, overrideTaxAmount);
			_items.Add(item);
			RecalculateTotals();
			return item;
		}

		public void RemoveItem(Guid itemId)
		{
			var idx = _items.FindIndex(i => i.Id == itemId);
			if (idx >= 0)
			{
				_items.RemoveAt(idx);
				RecalculateTotals();
			}
		}

		public void RecalculateTotals()
		{
			TotalAmount = _items.Sum(i => i.Amount);
			TotalTax = _items.Sum(i => i.TaxAmount);
			TotalWithTax = _items.Sum(i => i.TotalWithTax);
		}

		public void ReplaceItems(IEnumerable<ExpensePaymentItem> items)
		{
			_items.Clear();
			_items.AddRange(items);
			RecalculateTotals();
		}

		public void Submit() => Status = ExpensePaymentStatus.Submitted;
		public void Approve() => Status = ExpensePaymentStatus.Approved;
		public void Reject() => Status = ExpensePaymentStatus.Rejected;
		public void Cancel() => Status = ExpensePaymentStatus.Cancelled;
		public void ReadyForOutgoingPayment() => Status = ExpensePaymentStatus.ReadyForPayment;
		public void PartiallyPaid() => Status = ExpensePaymentStatus.PartiallyPaid;
		public void FullyPaid() => Status = ExpensePaymentStatus.FullyPaid;

		public void AddAttachment(string objectKey, string fileName, long size, string? url, Guid? fileId)
		{
			_attachments.Add(new ExpensePaymentAttachment(Id, objectKey, fileName, size, url, fileId));
		}

		public void LinkWorkflow(Guid instanceId) => CurrentWorkflowInstanceId = instanceId;
		public void UnlinkWorkflow() => CurrentWorkflowInstanceId = null;

		public decimal TotalOutgoing => _outgoingPayments.Sum(o => o.OutgoingAmount);

		// === Tổng còn lại chưa chi ===
		public decimal RemainingAmount => TotalWithTax - TotalOutgoing;
		public OutgoingPayment AddOutgoingPayment(
			string name, string bankName, string accountNumber, string beneficiaryName,
			decimal amount, DateTime dueDate, Guid outgoingBankAccountId,
			string? description = null 
		) {
			if (amount <= 0)
				throw new InvalidOperationException("Số tiền chi phải lớn hơn 0.");
			if (amount > RemainingAmount)
				throw new InvalidOperationException("Số tiền chi vượt quá tổng cần thanh toán.");

			var outgoing = new OutgoingPayment(
				name, amount,
				bankName, accountNumber, beneficiaryName,
				dueDate, outgoingBankAccountId, this.Id,
				description
			);

			_outgoingPayments.Add(outgoing);

			// Nếu tổng chi đã đủ
			if (RemainingAmount == 0)
				Status = ExpensePaymentStatus.FullyPaid;

			return outgoing;
		}

		public void UpdateOutgoingAmountPaid(IEnumerable<OutgoingPayment> outgoingPayments)
		{
			OutgoingAmountPaid = outgoingPayments.Where(x => x.Status == OutgoingPaymentStatus.Created).Sum(x => x.OutgoingAmount);
		}

		public void RecalculateRemaining()
		{
			RemainingOutgoingAmount = TotalWithTax - OutgoingAmountPaid;
		}
	}
}

using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePayment : AuditableEntity
	{
		private readonly List<ExpensePaymentItem> _items = new();

		private ExpensePayment() { } // EF

		public ExpensePayment(string name, PayeeType payeeType, DateTime paymentDate)
		{
			Id = Guid.NewGuid();
			Name = name.Trim();
			PayeeType = payeeType;
			PaymentDate = paymentDate;
			Status = ExpensePaymentStatus.Draft;
		}

		// Thông tin chung
		public string Name { get; private set; } = string.Empty;

		// Đối tượng thụ hưởng
		public PayeeType PayeeType { get; private set; }
		public Guid? SupplierId { get; private set; }   // null nếu Employee
		public Supplier? Supplier { get; private set; }  // optional nav

		// Thông tin chuyển khoản (ghi nhận “snapshot” tại thời điểm tạo)
		public string BankName { get; private set; } = string.Empty;
		public string AccountNumber { get; private set; } = string.Empty;
		public string BeneficiaryName { get; private set; } = string.Empty;

		// Lịch thanh toán
		public DateTime PaymentDate { get; private set; }
		public bool HasGoodsReceipt { get; private set; }

		// Tổng cộng (được item cộng dồn)
		public decimal TotalAmount { get; private set; }     // 18,2
		public decimal TotalTax { get; private set; }        // 18,2
		public decimal TotalWithTax { get; private set; }    // 18,2

		// Trạng thái luồng duyệt/chi
		public ExpensePaymentStatus Status { get; private set; }

		// Dòng hạng mục
		public IReadOnlyCollection<ExpensePaymentItem> Items => _items;

		// Đính kèm (tùy dự án, có thể dùng bảng chung Attachment; ở đây minh họa entity riêng)
		private readonly List<ExpensePaymentAttachment> _attachments = new();
		public IReadOnlyCollection<ExpensePaymentAttachment> Attachments => _attachments;

		// Payment followers
		private readonly List<ExpensePaymentFollower> _followers = new();
		public IReadOnlyCollection<ExpensePaymentFollower> Followers => _followers;


		// ==== HÀM NGHIỆP VỤ ====
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

		public void SetPaymentDate(DateTime date) => PaymentDate = date;

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
		public void MarkPaid() => Status = ExpensePaymentStatus.Paid;

		public void AddAttachment(string objectKey, string fileName, long size, string? url, Guid? fileId)
		{
			_attachments.Add(new ExpensePaymentAttachment(Id, objectKey, fileName, size, url, fileId));
		}

		public void AddFollower(Guid userId)
		{
			if (_followers.Any(f => f.UserId == userId)) return;
			_followers.Add(new ExpensePaymentFollower(Id, userId));
		}

		public void RemoveFollower(Guid userId)
		{
			var idx = _followers.FindIndex(f => f.UserId == userId);
			if (idx >= 0) _followers.RemoveAt(idx);
		}

		public void ReplaceFollowers(IEnumerable<Guid> userIds)
		{
			var set = new HashSet<Guid>(userIds.Where(id => id != Guid.Empty));
			_followers.RemoveAll(f => !set.Contains(f.UserId));
			foreach (var uid in set)
				if (!_followers.Any(f => f.UserId == uid))
					_followers.Add(new ExpensePaymentFollower(Id, uid));
		}
	}
}

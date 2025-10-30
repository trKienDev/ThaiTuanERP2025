using System.ComponentModel.DataAnnotations.Schema;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ExpensePayment : AuditableEntity
	{
		private readonly List<ExpensePaymentItem> _items = new();
		private readonly List<ExpensePaymentAttachment> _attachments = new();
		private readonly List<OutgoingPayment> _outgoingPayments = new();

		private ExpensePayment() { } // EF

		public ExpensePayment(string name, PayeeType payeeType, DateTime dueDate, Guid managerApproverId, string? description)
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstInvalidEnumValue(payeeType, nameof(payeeType));
			Guard.AgainstDefault(managerApproverId, nameof(managerApproverId));

			Id = Guid.NewGuid();
			Name = name.Trim();
			PayeeType = payeeType;
			DueDate = dueDate;
			ManagerApproverId = managerApproverId;
			Status = ExpensePaymentStatus.Pending;
			Description = description?.Trim() ?? string.Empty;

			AddDomainEvent(new ExpensePaymentCreatedEvent(this));
		}

		#region Properties
		public string Name { get; private set; } = string.Empty;
		public string SubId { get; private set; } = default!;
		public PayeeType PayeeType { get; private set; }
		public Guid? SupplierId { get; private set; }
		public Supplier? Supplier { get; private set; }

		public string BankName { get; private set; } = string.Empty;
		public string AccountNumber { get; private set; } = string.Empty;
		public string BeneficiaryName { get; private set; } = string.Empty;

		public DateTime DueDate { get; private set; }
		public bool HasGoodsReceipt { get; private set; }
		public string? Description { get; private set; }

		public decimal TotalAmount { get; private set; }
		public decimal TotalTax { get; private set; }
		public decimal TotalWithTax { get; private set; }
		public decimal OutgoingAmountPaid { get; private set; } = 0;
		public decimal RemainingOutgoingAmount { get; private set; }

		public ExpensePaymentStatus Status { get; private set; }

		public IReadOnlyCollection<ExpensePaymentItem> Items => _items.AsReadOnly();
		public IReadOnlyCollection<ExpensePaymentAttachment> Attachments => _attachments.AsReadOnly();
		public IReadOnlyCollection<OutgoingPayment> OutgoingPayments => _outgoingPayments.AsReadOnly();

		public Guid? CurrentWorkflowInstanceId { get; private set; }
		public ApprovalWorkflowInstance? CurrentWorkflowInstance { get; private set; }
		public Guid ManagerApproverId { get; private set; }
		#endregion

		#region Domain Behaviors

		public void SetSubId(string subId)
		{
			Guard.AgainstNullOrWhiteSpace(subId, nameof(subId));
			SubId = subId.Trim();
		}

		public void SetSupplier(Guid? supplierId)
		{
			SupplierId = supplierId;
		}

		public void SetBankInfo(string bankName, string accountNumber, string beneficiaryName)
		{
			Guard.AgainstNullOrWhiteSpace(bankName, nameof(bankName));
			Guard.AgainstNullOrWhiteSpace(accountNumber, nameof(accountNumber));
			Guard.AgainstNullOrWhiteSpace(beneficiaryName, nameof(beneficiaryName));

			BankName = bankName.Trim();
			AccountNumber = accountNumber.Trim();
			BeneficiaryName = beneficiaryName.Trim();
		}

		public ExpensePaymentItem AddItem(string itemName, int quantity, decimal unitPrice, decimal taxRate,
			Guid? budgetCodeId, Guid? cashoutCodeId, Guid? invoiceId = null, decimal? overrideTaxAmount = null)
		{
			var item = new ExpensePaymentItem(Id, itemName, quantity, unitPrice, taxRate, budgetCodeId, cashoutCodeId, invoiceId, overrideTaxAmount);
			_items.Add(item);
			RecalculateTotals();
			return item;
		}

		public void Submit()
		{
			if (!_items.Any())
				throw new DomainException("Không thể gửi duyệt phiếu không có hạng mục chi.");
			Status = ExpensePaymentStatus.Submitted;
			AddDomainEvent(new ExpensePaymentSubmittedEvent(this));
		}

		public void Approve()
		{
			Status = ExpensePaymentStatus.Approved;
			AddDomainEvent(new ExpensePaymentApprovedEvent(this));
		}

		public void Reject(string reason)
		{
			Status = ExpensePaymentStatus.Rejected;
			AddDomainEvent(new ExpensePaymentRejectedEvent(this, reason));
		}

		public void Cancel()
		{
			Status = ExpensePaymentStatus.Cancelled;
			AddDomainEvent(new ExpensePaymentCancelledEvent(this));
		}

		public void ReadyForOutgoingPayment()
		{
			Status = ExpensePaymentStatus.ReadyForPayment;
			AddDomainEvent(new ExpensePaymentReadyForPaymentEvent(this));
		}

		public void FullyPaid()
		{
			Status = ExpensePaymentStatus.FullyPaid;
			AddDomainEvent(new ExpensePaymentFullyPaidEvent(this));
		}

		#endregion
	}
}

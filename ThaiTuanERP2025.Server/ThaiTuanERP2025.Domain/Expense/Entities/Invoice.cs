using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Events.Invoices;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class Invoice : AuditableEntity
	{
		private readonly List<InvoiceFile> _files = new();

		private Invoice() { } // EF only
		public Invoice(
			string invoiceNumber,
			string invoiceName,
			DateTime issueDate,
			string sellerTaxCode,
			decimal totalAmount,
			decimal totalTax,
			string? sellerName = null,
			string? sellerAddress = null,
			string? buyerName = null,
			string? buyerTaxCode = null,
			string? buyerAddress = null
		)
		{
			Guard.AgainstNullOrWhiteSpace(invoiceNumber, nameof(invoiceNumber));
			Guard.AgainstNullOrWhiteSpace(invoiceName, nameof(invoiceName));
			Guard.AgainstDefault(issueDate, nameof(issueDate));
			Guard.AgainstNullOrWhiteSpace(sellerTaxCode, nameof(sellerTaxCode));

			Id = Guid.NewGuid();
			InvoiceNumber = invoiceNumber.Trim();
			InvoiceName = invoiceName.Trim();
			IssueDate = issueDate;
			SellerTaxCode = sellerTaxCode.Trim();

			SellerName = sellerName;
			SellerAddress = sellerAddress;
			BuyerName = buyerName;
			BuyerTaxCode = buyerTaxCode;
			BuyerAddress = buyerAddress;

			TotalAmount = totalAmount;
			TotalTax = totalTax;
			TotalWithTax = totalAmount + totalTax;
			IsDraft = true;

			AddDomainEvent(new InvoiceCreatedEvent(this));
		}

		#region Properties
		public string InvoiceNumber { get; private set; } = default!;
		public string InvoiceName { get; private set; } = default!;
		public DateTime IssueDate { get; private set; }
		public DateTime? PaymentDate { get; private set; }

		// Seller
		public string? SellerName { get; private set; }
		public string SellerTaxCode { get; private set; } = default!;
		public string? SellerAddress { get; private set; }

		// Buyer
		public string? BuyerName { get; private set; }
		public string? BuyerTaxCode { get; private set; }
		public string? BuyerAddress { get; private set; }

		public bool IsDraft { get; private set; }

		public decimal TotalAmount { get; private set; }
		public decimal TotalTax { get; private set; }
		public decimal TotalWithTax { get; private set; }

		public IReadOnlyCollection<InvoiceFile> Files => _files.AsReadOnly();

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }
		#endregion

		#region Domain Behaviors
		public void MarkAsPaid(DateTime paymentDate)
		{
			if (PaymentDate != null)
				throw new DomainException("Hóa đơn đã được thanh toán.");

			PaymentDate = paymentDate;
			AddDomainEvent(new InvoicePaidEvent(this));
		}

		public void MarkFinalized()
		{
			if (!IsDraft) return;
			IsDraft = false;
			AddDomainEvent(new InvoiceFinalizedEvent(this));
		}

		public void MarkAsDraft()
		{
			IsDraft = true;
		}

		public void RecalculateTotals(decimal totalAmount, decimal totalTax)
		{
			if (totalAmount < 0 || totalTax < 0)
				throw new DomainException("Giá trị tổng không hợp lệ.");

			TotalAmount = totalAmount;
			TotalTax = totalTax;
			TotalWithTax = totalAmount + totalTax;
		}

		public void AddFile(Guid fileId, bool isMain = false)
		{
			Guard.AgainstDefault(fileId, nameof(fileId));

			_files.Add(new InvoiceFile(Id, fileId, isMain));
			AddDomainEvent(new InvoiceFileAddedEvent(Id, fileId.ToString()));
		}
		#endregion
	}
}

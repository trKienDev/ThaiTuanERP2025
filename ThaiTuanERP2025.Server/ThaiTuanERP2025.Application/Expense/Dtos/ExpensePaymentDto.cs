using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record ExpensePaymentDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;

		public PayeeType PayeeType { get; init; }
		public Guid? SupplierId { get; init; }
		public Supplier? Supplier { get; init; }

		public string BankName { get; init; } = string.Empty;
		public string AccountNumber { get; init; } = string.Empty;	
		public string BeneficiaryName { get; init; } = string.Empty;

		public DateTime PaymentDate { get; init; }
		public bool HasGoodsReceipt { get; init; }

		public decimal TotalAmount { get; init; }
		public decimal TotalTax { get; init; }
		public decimal TotalWithTax { get; init; }

		public ExpensePaymentStatus Status { get; init; }	
		
		public IReadOnlyCollection<ExpensePaymentItem> Items { get; init; } = [];
		public IReadOnlyCollection<ExpensePaymentAttachment> Attachments { get; init; } = [];
		public IReadOnlyCollection<ExpensePaymentFollower> Followers { get; init; } = [];
	}

	public sealed record ExpensePaymentRequest {
		public string Name { get; init; } = string.Empty;

		public PayeeType PayeeType { get; init; }
		public Guid? SupplierId { get; init; }
		public Supplier? Supplier { get; init; }

		public string BankName { get; init; } = string.Empty;
		public string AccountNumber { get; init; } = string.Empty;
		public string BeneficiaryName { get; init; } = string.Empty;

		public DateTime PaymentDate { get; init; }
		public bool HasGoodsReceipt { get; init; }

		public decimal TotalAmount { get; init; }
		public decimal TotalTax { get; init; }
		public decimal TotalWithTax { get; init; }

		public ExpensePaymentStatus Status { get; init; }

		public IReadOnlyCollection<ExpensePaymentItemRequest> Items { get; init; } = [];
		public IReadOnlyCollection<ExpensePaymentAttachmentRequest> Attachments { get; init; } = [];
		public IReadOnlyCollection<Guid> FollowerIds { get; init; } = [];
	}
}

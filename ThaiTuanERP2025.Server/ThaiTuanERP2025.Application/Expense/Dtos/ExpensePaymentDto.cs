using ThaiTuanERP2025.Application.Account.Dtos;
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
		public string? Description { get; init; } = string.Empty;

		public decimal TotalAmount { get; init; }
		public decimal TotalTax { get; init; }
		public decimal TotalWithTax { get; init; }

		public ExpensePaymentStatus Status { get; init; }	
		
		public IReadOnlyCollection<ExpensePaymentItem> Items { get; init; } = [];
		public IReadOnlyCollection<ExpensePaymentAttachment> Attachments { get; init; } = [];
		public IReadOnlyCollection<ExpensePaymentFollower> Followers { get; init; } = [];
	}

	public sealed record ExpensePaymentRequest
	{
		public string Name { get; init; } = string.Empty;

		public PayeeType PayeeType { get; init; }
		public Guid? SupplierId { get; init; }

		public string BankName { get; init; } = string.Empty;
		public string AccountNumber { get; init; } = string.Empty;
		public string BeneficiaryName { get; init; } = string.Empty;

		public DateTime PaymentDate { get; init; }
		public bool HasGoodsReceipt { get; init; }
		public string? Description { get; init; } = string.Empty;

		public decimal TotalAmount { get; init; }
		public decimal TotalTax { get; init; }
		public decimal TotalWithTax { get; init; }

		public ExpensePaymentStatus Status { get; init; }

		public IReadOnlyCollection<ExpensePaymentItemRequest> Items { get; init; } = [];
		public IReadOnlyCollection<ExpensePaymentAttachmentRequest> Attachments { get; init; } = [];
		public IReadOnlyCollection<Guid> FollowerIds { get; init; } = [];

		public string ManagerApproverId { get; init; } = default!;
	}

	public sealed record ExpensePaymentDetailDto {
		// Payment core
		public Guid Id { get; init; }
		public string Name { get; init; } = default!;
		public string SubId { get; init; } = default!;
		public DateTime PaymentDate { get; init; }
		public bool HasGoodsReceipt { get; init; }
		public decimal TotalAmount { get; init; }
		public decimal TotalTax { get; init; }
		public decimal TotalWithTax { get; init; }
		public int Status { get; init; }

		// Creator
		public Guid CreatedByUserId { get; init; }
		public string CreatedByUsername { get; init; } = string.Empty;
		public UserDto CreatedByUser { get; init; } = default!;		

		// Supplier
		public Guid? SupplierId { get; init; }
		public SupplierDto? Supplier { get; init; }

		// Bank info (nhập trực tiếp trên chứng từ)
		public string BankName { get; init; } = string.Empty;
		public string AccountNumber { get; init; } = string.Empty;
		public string BeneficiaryName { get; init; } = string.Empty;
		public string Description { get; init; } = string.Empty;

		public DateTime CreatedDate { get; init; }

		// Items
		public IReadOnlyList<ExpensePaymentItemDetailDto> Items { get; init; } = Array.Empty<ExpensePaymentItemDetailDto>();
		public IReadOnlyList<ExpensePaymentAttachmentDto> Attachments { get; init; } = Array.Empty<ExpensePaymentAttachmentDto>();

		// Followers
		public IReadOnlyList<ExpensePaymentFollowersDto> Followers { get; init; } = Array.Empty<ExpensePaymentFollowersDto>();

		// Workflow
		public ApprovalWorkflowInstanceDetailDto? WorkflowInstanceDetail { get; init; } = default!;
	}
}

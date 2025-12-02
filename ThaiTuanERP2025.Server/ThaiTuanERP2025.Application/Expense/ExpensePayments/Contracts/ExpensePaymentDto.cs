using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts
{
	public sealed record ExpensePaymentDto
	{

	}

	public sealed record ExpensePaymentLookupDto
	{
		public Guid Id { get; init; } = Guid.Empty;
		public string Name { get; init; } = string.Empty;	 
		public ExpensePaymentStatus Status { get; init; }
		public decimal TotalAmount { get; init; }
		public decimal TotalTax { get; init; }
		public decimal TotalWithTax { get; init; }
		public decimal OutgoingAmountPaid { get; init; }
		public decimal RemainingOutgoingAmount { get; init; }
		public DateTime CreatedAt { get; init; }
		public Guid CreatedByUserId { get; init; }
		public UserBriefAvatarDto CreatedByUser { get; init; } = new UserBriefAvatarDto();
	}

	public sealed record ExpensePaymentDetailDto
	{
		public Guid Id { get; init; } = Guid.Empty;
		public string Name { get; init; } = string.Empty;
		public string SubId { get; init; } = string.Empty;
		public string? Description { get; init; } = string.Empty;
		public DateTime DueAt { get; init; }
		public bool HasGoodsReceipt { get; init; }
		public ExpensePaymentStatus Status { get; init; }
		public ExpenseWorkflowInstanceDetailDto? WorkflowInstance { get; init; }
		public IReadOnlyList<ExpensePaymentItemLookupDto> Items { get; init; } = Array.Empty<ExpensePaymentItemLookupDto>();
		public IReadOnlyList<ExpensePaymentAttachmentDto> Attachments { get; init; } = Array.Empty<ExpensePaymentAttachmentDto>();

		public ExpensePayeeType PayeeType { get; init; }
		public Guid? SupplierId { get; init; }
		public string? SupplierName { get; init; } 

		public string BankName { get; init; } = string.Empty;
		public string AccountNumber { get; init; } = string.Empty;
		public string BeneficiaryName { get; init; } = string.Empty;

		public decimal TotalAmount { get; init; }
		public decimal TotalTax { get; init; }
		public decimal TotalWithTax { get; init; }
		public decimal OutgoingAmountPaid { get; init; }
		public decimal RemainingOutgoingAmount { get; init; }

		public Guid CreatedByUserId { get; init; }
		public UserBriefAvatarDto CreatedByUser { get; init; } = new UserBriefAvatarDto();
		public DateTime CreatedAt { get; init; }

		public IReadOnlyList<UserBriefAvatarDto> Followers { get; set; } = Array.Empty<UserBriefAvatarDto>();	
	}
}

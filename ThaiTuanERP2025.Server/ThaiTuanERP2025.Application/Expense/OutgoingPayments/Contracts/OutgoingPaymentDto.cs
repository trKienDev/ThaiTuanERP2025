using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts
{
        public sealed record OutgoingPaymentDto
        {
                public string Name { get; init; } = string.Empty;
                public string? Description { get; init; }
                public string BankName { get; init; } = string.Empty;
                public string AccountNumber { get; init; } = string.Empty;
                public string BeneficiaryName { get; init; } = string.Empty;
                public decimal OutgoingAmount { get; init; }
                public DateTime DueAt { get; init; }
                public Guid OutgoingBankAccountId { get; init; }
                public Guid ExpensePaymentId { get; init; }
                public OutgoingPaymentStatus Status { get; init; }
        }

        public sealed record OutgoingPaymentBriefDto
        {
                public Guid Id { get; init; } = Guid.Empty;
                public string Name { get; init; } = string.Empty;
                public OutgoingPaymentStatus Status { get; init; }
                public DateTime PostingAt { get; init; }
                public decimal OutgoingAmount { get; init; }
        }

        public sealed record OutgoingPaymentLookupDto
        {
                public Guid Id { get; init; }
                public string Name { get; init; } = string.Empty;
                public OutgoingPaymentStatus Status { get; init; }
                public DateTime DueAt { get; init; }
                public DateTime PostingAt { get; init; }
                public decimal OutgoingAmount { get; init; }
                
                public Guid ExpensePaymentId { get; init; } = Guid.Empty;
                public string ExpensePaymentName { get; init; } = string.Empty;

                public Guid OutgoingBankAccountId { get; init; } = Guid.Empty;
                public string OutgoingBankAccountName { get; init; } = string.Empty;  

                public Guid? SupplierId { get; init; }
                public string? SupplierName { get; init; } = string.Empty;
	}

        public sealed record OutgoingPaymentDetailDto
        {
                public Guid Id { get; init; }
                public string Name { get; init; } = string.Empty;
                public string SubId { get; init; } = string.Empty;
                public string? Description { get; init; }
                public decimal OutgoingAmount { get; init; }
                public OutgoingPaymentStatus Status { get; init; }

                public string BankName { get; init; } = string.Empty;
                public string AccountNumber { get; init; } = string.Empty;
                public string BeneficiaryName { get; init; } = string.Empty;

                public DateTime PostingAt { get; init; }
                public DateTime PaymentAt { get; init; }
                public DateTime DueAt { get; init; }

		public Guid ExpensePaymentId { get; init; } = Guid.Empty;
		public string ExpensePaymentName { get; init; } = string.Empty;
                public decimal ExpensePaymentAmount { get; init; }
                public IReadOnlyList<ExpensePaymentItemLookupDto> ExpensePaymentItems { get; init; } = Array.Empty<ExpensePaymentItemLookupDto>();


                public Guid OutgoingBankAccountId { get; init; } = Guid.Empty;
		public string OutgoingBankAccountName { get; init; } = string.Empty;

		public Guid? SupplierId { get; init; }
		public string? SupplierName { get; init; } = string.Empty;

                public IReadOnlyList<UserBriefAvatarDto> Followers { get; init; } = Array.Empty<UserBriefAvatarDto>();

                public Guid CreatedByUserId { get; init; }
                public UserBriefAvatarDto CreatedByUser { get; init; } = new UserBriefAvatarDto();
	}
}

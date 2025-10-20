using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public record OutgoingPaymentDto(
		Guid Id,
		string Name,
		string Description,
		decimal OutgoingAmount,
		string BankName,
		string AccountNumber,
		string BeneficiaryName,
		DateTime PostingDate,
		DateTime PaymentDate,
		DateTime DueDate,
		Guid OutgoingBankAccountId,
		Guid ExpensePaymentId,
		OutgoingPaymentStatus Status
	);

	public sealed record OutgoingPaymentRequest (
		string Name,
		string Description,
		PayeeType PayeeType,
		Guid? SupplierId,
		Guid? EmployeeId,
		decimal OutgoingAmount,
		string BankName,
		string AccountNumber,
		string BeneficiaryName,
		DateTime DueDate,
		Guid OutgoingBankAccountId,
		Guid ExpensePaymentId
	);

	public record OutgoingPaymentSummaryDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string Description { get; init; } = string.Empty;
		public decimal OutgoingAmount { get; init; }

		public string BankName { get; init; } = string.Empty;
		public string AccountNumber { get; init; } = string.Empty;
		public string BeneficiaryName { get; init; } = string.Empty;

		public DateTime PostingDate { get; init; }
		public DateTime PaymentDate { get; init; }
		public DateTime DueDate { get; init; }

		public Guid OutgoingBankAccountId { get; init; }
		public string OutgoingBankAccountName { get; init; } = string.Empty;

		public Guid ExpensePaymentId { get; init; }
		public string ExpensePaymentName { get; init; } = string.Empty;

		public OutgoingPaymentStatus Status { get; init; }
	};

	public record OutgoingPaymentDetailDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = string.Empty;
		public string SubId { get; init; } = default!;
		public string Description { get; init; } = string.Empty;
		public decimal OutgoingAmount { get; init; }

		public string BankName { get; init; } = string.Empty;
		public string AccountNumber { get; init; } = string.Empty;
		public string BeneficiaryName { get; init; } = string.Empty;

		public DateTime PostingDate { get; init; }
		public DateTime PaymentDate { get; init; }
		public DateTime DueDate { get; init; }

		public Guid OutgoingBankAccountId { get; init; }
		public OutgoingBankAccountDto OutgoingBankAccount { get; init; } = default!;

		public Guid ExpensePaymentId { get; init; }
		public ExpensePaymentDetailDto ExpensePayment { get; init; } = default!;

		public OutgoingPaymentStatus Status { get; init; }

		public Guid? SupplierId { get; init; }
		public SupplierDto? Supplier { get; init; } = default!;

		public Guid? EmployeeId { get; init; }
		public UserDto? Employee { get; init; } = default!;

		public UserDto CreatedByUser { get; init; } = default!;
		public DateTime CreatedDate { get; init; }
	};
}

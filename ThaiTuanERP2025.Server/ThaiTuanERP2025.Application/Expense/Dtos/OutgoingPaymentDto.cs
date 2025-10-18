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
		decimal OutgoingAmount,
		string BankName,
		string AccountNumber,
		string BeneficiaryName,
		DateTime DueDate,
		Guid OutgoingBankAccountId,
		Guid ExpensePaymentId
	);

	public record OutgoingPaymentDetailDto (
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
		OutgoingPaymentStatus Status,
		
		Guid CreatedByUserId,
		UserDto CreatedByUser
	);
}

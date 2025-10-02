namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed record BankAccountDto(
		Guid Id, 
		string BankName, string AccountNumber,  string BeneficiaryName, bool IsActive,
		Guid? UserId, Guid? SupplierId 
	);

	public sealed record CreateUserBankAccountRequest(
		Guid UserId,
		string BankName, string AccountNumber, string BeneficiaryName
	);

	public sealed record CreateSupplierBankAccountRequest (
		Guid SupplierId,
		string BankName, string AccountNumber, string BeneficiaryName
	);

	public sealed record UpdateBankAccountRequest (
		string BankName, string AccountNumber, string BeneficiaryName, bool IsActive
	);
}

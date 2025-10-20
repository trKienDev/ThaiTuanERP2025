namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public record OutgoingBankAccountDto (
		Guid Id, 
		string Name,
		string BankName, 
		string AccountNumber,
		string OwnerName, 
		bool IsActive
	);

	public sealed record OutgoingBankAccountRequest (
		string Name,
		string BankName, 
		string AccountNumber,
		string OwnerName
	);
}

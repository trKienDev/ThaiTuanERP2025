namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record BankAccountDto (
		Guid Id,
		string AccountNumber,
		string BankName,
		string? AccountHolder,
		string? OwnerName,
		bool IsActive,
		DateTime CreatedDate
	);
}

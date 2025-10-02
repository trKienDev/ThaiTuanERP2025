namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record CashoutCodeDto(
		Guid Id, string Code, string Name, string? Description, bool IsActive,
		Guid CashoutGroupId, string CashoutGroupCode, string CashoutGroupName,
		Guid PostingLedgerAccountId, string PostingLedgerAccountNumber, string PostingLedgerAccountName
	);
}

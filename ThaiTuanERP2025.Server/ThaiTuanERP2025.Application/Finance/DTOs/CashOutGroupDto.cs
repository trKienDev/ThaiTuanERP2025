namespace ThaiTuanERP2025.Application.Finance.DTOs
{
	public record CashoutGroupDto(
		Guid Id, string Code, string Name, string? Description, bool IsActive,
		Guid? ParentId, string? Path, int Level,
		List<CashoutGroupDto>? Children
	);
}

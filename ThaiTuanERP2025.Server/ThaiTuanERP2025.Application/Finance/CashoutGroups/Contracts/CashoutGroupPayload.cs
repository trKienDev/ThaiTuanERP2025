namespace ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts
{
	public sealed record CashoutGroupPayload(
		string Name, 
		Guid? ParentId,
		string? Description
	);
}

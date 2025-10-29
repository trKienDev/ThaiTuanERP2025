namespace ThaiTuanERP2025.Application.Account.RBAC.Requests
{
	public sealed record RoleRequest
	{
		public string Name { get; init; } = default!;
		public string Description { get; init; } = string.Empty;
	}
}

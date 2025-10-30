namespace ThaiTuanERP2025.Application.Account.RBAC.Roles
{
	public sealed record RoleDto
	{
		public Guid Id { get; init; }
		public string Name { get; set; } = default!;
		public string Description { get; set; } = string.Empty;
		public bool IsActive { get; set; }
	}
}

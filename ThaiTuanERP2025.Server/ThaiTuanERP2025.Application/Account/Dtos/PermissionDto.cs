namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public record PermissionDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = default!;
		public string Code { get; set; } = default!;
		public string Description { get; set; } = string.Empty;
	}

	public sealed record AssignPermissionToRoleRequest
	{
		public List<Guid> PermissionIds { get; set; } = new();
	}

}

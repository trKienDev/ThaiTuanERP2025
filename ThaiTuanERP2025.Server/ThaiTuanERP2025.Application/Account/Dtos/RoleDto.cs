namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public record RoleDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = default!;
		public string Description { get; set; } = string.Empty;
	}
}

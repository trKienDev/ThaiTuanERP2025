namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public record PermissionDto
	{
		public Guid Id { get; set; }
		public string Code { get; set; } = default!;
		public string Description { get; set; } = string.Empty;
	}
}

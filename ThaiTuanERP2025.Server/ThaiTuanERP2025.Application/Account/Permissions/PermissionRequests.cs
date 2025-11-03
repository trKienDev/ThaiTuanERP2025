namespace ThaiTuanERP2025.Application.Account.Permissions
{
	public sealed record PermissionRequest
	{
		public string Name { get; set; } = default!;
		public string Code { get; set; } = default!;
		public string Description { get; set; } = string.Empty;
	}

}

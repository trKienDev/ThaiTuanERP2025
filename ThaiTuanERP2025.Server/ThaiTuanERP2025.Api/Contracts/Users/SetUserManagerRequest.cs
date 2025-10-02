namespace ThaiTuanERP2025.Api.Contracts.Users
{
	public sealed class SetUserManagerRequest
	{
		public List<Guid> ManagerIds { get; set; } = new();
		public Guid? PrimaryManagerId { get; set; }
	}
}

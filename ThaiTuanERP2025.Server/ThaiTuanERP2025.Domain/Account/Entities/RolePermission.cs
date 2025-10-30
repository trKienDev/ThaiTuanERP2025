namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class RolePermission
	{
		public Guid RoleId { get; private set; }
		public Role Role { get; private set; } = default!;

		public Guid PermissionId { get; private set; }
		public Permission Permission { get; private set; } = default!;

		private RolePermission() { }

		public RolePermission(Guid roleId, Guid permissionId)
		{
			RoleId = roleId;
			PermissionId = permissionId;
		}
	}
}

using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class RolePermission
	{
		public Guid RoleId { get; private set; }
		public Guid PermissionId { get; private set; }

		private RolePermission() { } // EF only

		internal RolePermission(Guid roleId, Guid permissionId)
		{
			Guard.AgainstDefault(roleId, nameof(roleId));
			Guard.AgainstDefault(permissionId, nameof(permissionId));

			RoleId = roleId;
			PermissionId = permissionId;
		}

		// Optional: equality based on composite key
		public override bool Equals(object? obj)
		{
			if (obj is not RolePermission other) return false;
			return RoleId == other.RoleId && PermissionId == other.PermissionId;
		}

		public override int GetHashCode() => HashCode.Combine(RoleId, PermissionId);
	}
}

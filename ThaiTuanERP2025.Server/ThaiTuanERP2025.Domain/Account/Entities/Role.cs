using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Role : AuditableEntity
	{
		public string Name { get; private set; } = default!;
		public string Description { get; private set; } = string.Empty;
		public bool IsActive { get; private set; } = true;

		public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();
		public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

		private Role() { } // EF

		public Role(string name, string description = "")
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Tên vai trò không hợp lệ", nameof(name));

			Id = Guid.NewGuid();
			Name = name;
			Description = description;
			IsActive = true;
		}

		public void Deactivate() => IsActive = false;
		public void Activate() => IsActive = true;

		public void Update(string name, string description)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Tên vai trò không hợp lệ", nameof(name));

			Name = name;
			Description = description;
		}
	}
}

using System.Text.RegularExpressions;
using ThaiTuanERP2025.Domain.Account.Events.Permissions;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Permission : AuditableEntity
	{
		private readonly List<RolePermission> _rolePermissions = new();
		private static readonly Regex CodePattern = new(@"^[a-z0-9]+\.[a-z0-9.]+$", RegexOptions.Compiled);

		private Permission() { } // EF only

		public Permission(string name, string code, string description = "")
		{
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));
			if (!CodePattern.IsMatch(code))
				throw new ArgumentException("Mã quyền phải có dạng 'module.action', ví dụ 'expense.create'");

			Id = Guid.NewGuid();
			Name = name.Trim();
			Code = code.Trim().ToLowerInvariant();
			Description = description?.Trim() ?? string.Empty;
			IsActive = true;

			AddDomainEvent(new PermissionCreatedEvent(this));
		}

		public string Name { get; private set; } = string.Empty;
		public string Code { get; private set; } = string.Empty;
		public string Description { get; private set; } = string.Empty;
		public bool IsActive { get; private set; } = true;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

		#region Domain Behaviors
		public void Rename(string newName)
		{
			Guard.AgainstNullOrWhiteSpace(newName, nameof(newName));
			Name = newName.Trim();
			AddDomainEvent(new PermissionRenamedEvent(this));
		}

		public void UpdateDescription(string newDescription)
		{
			Description = newDescription?.Trim() ?? string.Empty;
			AddDomainEvent(new PermissionDescriptionUpdatedEvent(this));
		}

		public void Deactivate()
		{
			if (!IsActive) return;
			IsActive = false;
			AddDomainEvent(new PermissionDeactivatedEvent(this));
		}

		public void Activate()
		{
			if (IsActive) return;
			IsActive = true;
			AddDomainEvent(new PermissionActivatedEvent(this));
		}
		#endregion
	}
}

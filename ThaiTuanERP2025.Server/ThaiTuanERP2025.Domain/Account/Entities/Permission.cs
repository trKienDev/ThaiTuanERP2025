using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Permission : AuditableEntity
	{
		public string Name { get; private set; } = default!; // ví dụ: "Tạo chi phí"
		public string Code { get; private set; } = default!; // ví dụ: "expense.create"
		public string Description { get; private set; } = string.Empty;

		public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

		private Permission() { }

		public Permission( string name, string code, string description = "")
		{
			if(string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Tên quyền không hợp lệ", nameof(name));

			if (string.IsNullOrWhiteSpace(code))
				throw new ArgumentException("Mã quyền không hợp lệ", nameof(code));

			Id = Guid.NewGuid();
			Name = name;
			Code = code;
			Description = description;
		}
	}
}

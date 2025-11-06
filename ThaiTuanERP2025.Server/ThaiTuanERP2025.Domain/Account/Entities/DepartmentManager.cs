using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class DepartmentManager : BaseEntity
	{
		#region EF Constructor
		private DepartmentManager() { }
		public DepartmentManger (Guid departmentId, Guid userId, bool isPrimary = false) {
			Guard.AgainstDefault(departmentId, nameof(departmentId));
			Guard.AgainstDefault(userId, nameof(userId));
			DepartmentId = departmentId;
			UserId = userId;
			IsPrimary = isPrimary;
		}
		#endregion

		#region Properties
		public Guid DepartmentId { get; private set; }
		public Department Department { get; private set; } = default!;
		public Guid UserId { get; private set; }
		public User User { get; private set; } = default!;
		public bool IsPrimary { get; private set; }
		#endregion

		#region Domain behaviors
		public void SetPrimary() => IsPrimary = true;
		public void UnsetPrimary() => IsPrimary = false;
		#endregion
	}
}

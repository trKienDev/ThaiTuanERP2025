using ThaiTuanERP2025.Domain.Shared;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class DepartmentManager
	{
		#region Properties
		public Guid DepartmentId { get; private set; }
		public Department Department { get; private set; } = default!;
		public Guid UserId { get; private set; }
		public User User { get; private set; } = default!;
		public bool IsPrimary { get; private set; }
		#endregion

		#region EF Constructor
		private DepartmentManager() { }
		public DepartmentManager (Guid departmentId, Guid userId, bool isPrimary = false) {
			Guard.AgainstDefault(departmentId, nameof(departmentId));
			Guard.AgainstDefault(userId, nameof(userId));
			DepartmentId = departmentId;
			UserId = userId;
			IsPrimary = isPrimary;
		}
		#endregion

		

		#region Domain behaviors
		public void SetPrimary() => IsPrimary = true;
		public void UnsetPrimary() => IsPrimary = false;
		#endregion
	}
}

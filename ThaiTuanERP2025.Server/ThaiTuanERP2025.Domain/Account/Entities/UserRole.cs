using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class UserRole
	{
		public Guid UserId { get; private set; }
		public Guid RoleId { get; private set; }

		private UserRole() { } // EF

		internal UserRole(Guid userId, Guid roleId)
		{
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstDefault(roleId, nameof(roleId));

			if (userId == roleId)
				throw new DomainException("UserId và RoleId không hợp lệ.");

			UserId = userId;
			RoleId = roleId;
		}

		public override bool Equals(object? obj)
		    => obj is UserRole other && UserId == other.UserId && RoleId == other.RoleId;

		public override int GetHashCode() => HashCode.Combine(UserId, RoleId);
	}
}

﻿namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class UserRole
	{
		public Guid UserId { get; private set; }
		public User User { get; private set; } = default!;

		public Guid RoleId { get; private set; }
		public Role Role { get; private set; } = default!;

		private UserRole() { }

		public UserRole(Guid userId, Guid roleId)
		{
			UserId = userId;
			RoleId = roleId;
		}
	}
}

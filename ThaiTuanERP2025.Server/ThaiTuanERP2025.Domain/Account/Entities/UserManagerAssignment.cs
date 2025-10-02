namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class UserManagerAssignment
	{
		public Guid UserId { get; set; }
		public User User { get; set; } = null!;

		public Guid ManagerId {get; set; }
		public User Manager { get; set; } = null!;

		public bool IsPrimary { get; set; }

		public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
		public DateTime? RevokedAt { get; set; }
	}
}

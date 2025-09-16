using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Division : AuditableEntity
	{
		public string Name { get; private set; } = string.Empty;
		public string? Description { get; private set; } = string.Empty;
		public bool IsActive { get; private set; } = true;

		// head
		public Guid HeadUserId { get; private set; }
		public User HeadUser { get; private set; } = null!;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public ICollection<Department> Departments { get; private set; }
		private Division() {
			Departments = new List<Department>();
		}

		public Division(string name, string description, Guid headUserId)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tên không được để trống");
			Name = name;
			Description = description;
			IsActive = true;
			HeadUserId = headUserId;
			Departments = new List<Department>();
		}

		public void Rename(string newName)
		{
			if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Tên không được để trống");
			Name = newName;
		}

		public void SetHead(Guid userId) => HeadUserId = userId;
	}
}

using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Domain.Shared.Entities
{
	public class AuditableEntity : BaseEntity
	{
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public Guid? CreatedByUserId { get; set; } // Foreign Key
		public User? CreatedByUser { get; set; } = default!;

		public DateTime? ModifiedAt { get; set; }
		public Guid? ModifiedByUserId { get; set; } // Foreign Key
		public User? ModifiedByUser { get; set; } 

		// soft-delete
		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletedAt { get; set; } 
		public Guid? DeletedByUserId { get; set; } // Foreign Key
		public User? DeletedByUser { get; set; }
	}
}

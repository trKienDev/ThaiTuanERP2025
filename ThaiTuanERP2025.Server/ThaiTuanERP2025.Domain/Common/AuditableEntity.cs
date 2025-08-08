using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Common
{
	public class AuditableEntity : BaseEntity
	{
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public Guid CreatedByUserId { get; set; } // Foreign Key

		public DateTime? DateModified { get; set; }
		public Guid? ModifiedByUserId { get; set; } // Foreign Key
		
		// soft-delete
		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletedDate { get; set; } 
		public Guid? DeletedByUserId { get; set; } // Foreign Key
	}
}

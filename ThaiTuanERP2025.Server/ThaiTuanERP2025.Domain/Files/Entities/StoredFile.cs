using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Files.Entities
{
	public class StoredFile : AuditableEntity
	{
		// Vị trí trong MinIO
		public string Bucket { get; set; } = null!;
		public string ObjectKey { get; set; } = null!;  // vd: expense/invoices/yyyy/MM/{guid}.pdf

		// Thông tin File
		public string FileName { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public long Size { get; set; }	
		public string? Hash { get; set; } // tuỳ chọn: sha256 để chống trùng

		// Thông tin File
		public string Module { get; set; } = null!; // vd: "expense"
		public string Entity { get; set; } = null!; // vd: "invoice"
		public string? EntityId { get; set; } // id chứng từ

		public bool IsPublic { get; set; } = false;
	}
}

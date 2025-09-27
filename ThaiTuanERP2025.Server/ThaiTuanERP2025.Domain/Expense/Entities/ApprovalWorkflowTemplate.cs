using System.ComponentModel.DataAnnotations;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalWorkflowTemplate : AuditableEntity
	{
		[MaxLength(200)]
		public string Name { get; private set; } = string.Empty;

		/// <summary>
		/// Loại chứng từ: expense-payment / advance-payment / advance-settlement...
		/// Dùng string để dễ mở rộng (có thể chuyển sang enum + ValueConverter nếu bạn muốn).
		/// </summary>
		[MaxLength(100)]
		public string DocumentType { get; private set; } = string.Empty;

		/// <summary>
		/// Version của template (tăng khi thay đổi cấu trúc step).
		/// </summary>
		public int Version { get; private set; } = 1;
		public bool IsActive { get; private set; } = true;

		// Navigation
		public ICollection<ApprovalStepTemplate> Steps { get; private set; } = new List<ApprovalStepTemplate>();

		private ApprovalWorkflowTemplate() { }
		public ApprovalWorkflowTemplate(string name, int version = 1)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Name cannot be null or empty.", nameof(name));
			if (version < 1)
				throw new ArgumentOutOfRangeException(nameof(version), "Version must be at least 1.");
			Name = name;
			Version = version;
		}

		public void Activate() => IsActive = true;	
		public void Deactivate() => IsActive = false;

		/// <summary>Tăng version khi thay đổi bước (thêm/sửa/xoá/reorder).</summary>
		public void BumpVersion() => Version++;

		public void MarkDeleted() => IsDeleted = true;
	}
}

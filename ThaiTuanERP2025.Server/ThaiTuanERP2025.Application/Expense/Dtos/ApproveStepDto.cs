using System.ComponentModel.DataAnnotations;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class ApproveStepDto
	{
		/// Comment / ghi chú duyệt
		public string? Comment { get; set; }

		/// RowVersion để check optimistic concurrency
		[Required]
		public byte[] RowVersion { get; set; } = default!;
	}
}

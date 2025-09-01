using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class RejectStepDto
	{
		/// Lý do từ chối (nên bắt buộc để đảm bảo trace rõ ràng)
		[Required]
		public string Reason { get; set; } = default!;

		/// RowVersion để check optimistic concurrency
		[Required]
		public byte[] RowVersion { get; set; } = default!;
	}
}

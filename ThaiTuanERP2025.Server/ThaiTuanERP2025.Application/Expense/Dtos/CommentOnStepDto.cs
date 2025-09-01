namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public sealed class CommentOnStepDto
	{
		/// Nội dung comment 
		public string? Comment {  get; set; }	

		/// Danh sách file đính kèm
		public List<Guid>? AttachmentFileIds { get; set; }

	}
}

using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public record ApprovalStepDto {
		public string Name { get; set; } = string.Empty;
		public int SlaHours { get; set; }
		public string FlowType { get; set; } = string.Empty;
		List<Guid> ApproverIds { get; set; } = new List<Guid>();
	};
}

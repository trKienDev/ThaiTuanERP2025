using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts
{
	public sealed record ExpenseStepInstanceDto
	{
	}

	public sealed record ExpenseStepInstanceBriefDto
	{
		public int Order { get; init; }
		public ExpenseStepStatus Status { get; init; }
		public UserBriefAvatarDto? ApprovedByUser { get; init; } = null;
		public DateTime? DueAt { get; init; }
	}

	public sealed record ExpenseStepInstanceDetailDto
	{
		public int Order { get; init; }
		public ExpenseStepStatus Status { get; init; }
		public UserBriefAvatarDto? ApprovedByUser { get; init; } = null;
		public UserBriefAvatarDto? RejectedByUser { get; init; } = null;
		public DateTime? DueAt { get; init; }
		public List<Guid> ApproverIds { get; set; } = new();
	}
}

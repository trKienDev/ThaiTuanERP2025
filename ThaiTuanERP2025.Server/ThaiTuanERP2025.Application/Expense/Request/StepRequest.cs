namespace ThaiTuanERP2025.Application.Expense.Request
{
	public sealed class ApproveStepRequest {
		public string? Comment { get; init; }
	}

	public sealed class RejectStepRequest
	{
		public string Reason { get; init; } = null!;
	}

	public sealed class OverrideApproverRequest
	{
		public Guid NewApproverId { get; init; }
		public string? Reason { get; init; }
	}

	public sealed class ReopenStepRequest {
		public string? Reason { get; init; }
	}

}

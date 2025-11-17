namespace ThaiTuanERP2025.Application.Core.OutboxMessages
{
	public sealed record OutboxMessageDto
	{
		public string Message { get; init; } = string.Empty;
		public string Payload { get; init; } = string.Empty;
		public DateTime OccurredOnUtc { get; init; }
	}
}

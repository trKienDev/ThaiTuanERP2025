namespace ThaiTuanERP2025.Application.Common.Events
{
	/// <summary>
	/// Cung cấp khả năng publish các Application Events (Command Events) ra hệ thống (qua MediatR hoặc message bus).
	/// </summary>
	public interface IApplicationEventPublisher
	{
		/// <summary>
		/// Gửi (publish) một Application Event ngay lập tức.
		/// </summary>
		Task PublishAsync(IApplicationEvent @event, CancellationToken cancellationToken = default);
	}
}

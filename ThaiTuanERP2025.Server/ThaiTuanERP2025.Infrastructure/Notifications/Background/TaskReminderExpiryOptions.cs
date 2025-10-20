namespace ThaiTuanERP2025.Infrastructure.Notifications.Background
{
	public sealed class TaskReminderExpiryOptions
	{
		/// <summary>Khoảng quét (mặc định 1 phút)</summary>
		public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);

		/// <summary>Số bản ghi xử lý mỗi batch để tránh lock dài</summary>
		public int BatchSize { get; set; } = 500;
	}
}

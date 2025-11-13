namespace ThaiTuanERP2025.Domain.Shared.Utils
{
	public static class TimeZoneConverter
	{
		private static readonly TimeZoneInfo _vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

		public static DateTime ToVietnamTime(DateTime utcTime) => TimeZoneInfo.ConvertTimeFromUtc(utcTime, _vnTimeZone);
	}
}

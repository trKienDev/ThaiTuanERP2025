namespace ThaiTuanERP2025.Application.Common.Options
{
	public class DocumentSubIdOptions
	{
		/// <summary>
		/// Bảng ánh xạ DocumentType → digit (chuỗi số)
		/// </summary>
		public Dictionary<string, string> TypeDigits { get; init; } = new();
	}
}

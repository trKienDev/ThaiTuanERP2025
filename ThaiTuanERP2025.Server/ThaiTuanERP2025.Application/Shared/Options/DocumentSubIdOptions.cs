using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Shared.Options
{
	public class DocumentSubIdOptions
	{
		/// <summary>
		/// Bảng ánh xạ DocumentType → digit (chuỗi số)
		/// </summary>
		public Dictionary<DocumentType, string> TypeDigits { get; init; } = new();
	}
}

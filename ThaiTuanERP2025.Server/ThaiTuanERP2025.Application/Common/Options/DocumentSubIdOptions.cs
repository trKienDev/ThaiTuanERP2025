using ThaiTuanERP2025.Domain.Common.Enums;

namespace ThaiTuanERP2025.Application.Common.Options
{
	public class DocumentSubIdOptions
	{
		/// <summary>
		/// Bảng ánh xạ DocumentType → digit (chuỗi số)
		/// </summary>
		public Dictionary<DocumentType, string> TypeDigits { get; init; } = new();
	}
}

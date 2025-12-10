namespace ThaiTuanERP2025.Application.Shared.Models
{
	/// <summary>
	/// Yêu cầu phân trang/tra cứu chuẩn cho các endpoint /paged.
	/// Sort format: "field:asc" | "field:desc". Filters là cặp key/value (string).
	/// </summary>
	public class PagedRequest {
		/// <summary>Trang hiện tại (bắt đầu từ 1)</summary>
		public int PageIndex { get; set; } = 1;

		/// <summary>Kích thước trang (số item/trang)</summary>
		public int PageSize { get; set; } = 20;

		/// <summary>Từ khoá tìm kiếm chung (áp dụng per-entity)</summary>
		public string? Keyword { get; set; }

		/// <summary>Chuỗi sắp xếp "field:dir", ví dụ "policyName:asc", "rate:desc"</summary>
		public string? Sort { get; set; }

		/// <summary>Các bộ lọc tuỳ biến (ví dụ: isActive=true, typeId=...)</summary>
		public Dictionary<string, string?>? Filters { get; set; }

		/// <summary>
		/// Chuẩn hoá các giá trị đầu vào (chặn size âm/quá lớn, page < 1, trim keyword)
		/// Gọi hàm này ở Controller/Handler trước khi chuyển cho Repository.
		/// </summary>
		public void Normalize(int maxPageSize = 200)
		{
			if (PageIndex < 1) PageIndex = 1;
			if (PageSize < 1) PageSize = 1;
			if (PageSize > maxPageSize) PageSize = maxPageSize;
			if (Keyword != null) Keyword = Keyword.Trim();
			if (string.IsNullOrWhiteSpace(Sort)) Sort = null;
		}
	}
}

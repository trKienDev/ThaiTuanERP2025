using System.Net;

namespace ThaiTuanERP2025.Domain.Exceptions
{
	/// <summary>
	/// Dùng cho lỗi validate input. Chứa danh sách lỗi theo từng field.
	/// </summary>
	public class ValidationException : AppException
	{
		/// <summary>
		/// Key: tên field (ví dụ "Code", "Name"); Value: danh sách message lỗi cho field đó.
		/// </summary>
		public IDictionary<string, string[]> Errors { get; }

		/// <summary>
		/// Khởi tạo với danh sách lỗi; status 422 UnprocessableEntity.
		/// </summary>
		public ValidationException(IDictionary<string, string[]> errors) : base("One or more validation errors occurred.", (int)HttpStatusCode.UnprocessableEntity)
		{
			Errors = errors ?? new Dictionary<string, string[]>();
		}

		/// <summary>
		/// Khởi tạo với message tuỳ ý + danh sách lỗi; status 422 UnprocessableEntity.
		/// </summary>
		public ValidationException(string message, IDictionary<string, string[]> errors) : base(string.IsNullOrWhiteSpace(message) ? "One or more validation errors occurred." : message, (int)HttpStatusCode.UnprocessableEntity) {
			Errors = errors ?? new Dictionary<string, string[]>();
		}

		/// <summary>
		/// Khởi tạo nhanh với 1 field + 1 message; status 422 UnprocessableEntity.
		/// </summary>
		public ValidationException(string field, string errorMessage) : base("One or more validation errors occurred.", (int)HttpStatusCode.UnprocessableEntity)
		{
			Errors = new Dictionary<string, string[]>
			{
				[field ?? ""] = new[] { errorMessage }
			};
		}
	}
}

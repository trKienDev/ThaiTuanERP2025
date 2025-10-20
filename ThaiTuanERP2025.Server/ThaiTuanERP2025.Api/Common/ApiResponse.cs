namespace ThaiTuanERP2025.Presentation.Common
{
	public class ApiResponse<T>
	{
		public bool IsSuccess { get; set; }
		public string? Message { get; set; }
		public T? Data { get; set; } = default!;
		public List<string> Errors { get; set; } = new List<string>();

		public static ApiResponse<T> Success(T data, string? message = null) => new() { IsSuccess = true, Data = data, Message = message };

		public static ApiResponse<T> Fail(string message) => new() { IsSuccess = false, Message = message };

		public static ApiResponse<T> Fail(string message, IEnumerable<string> errors) => new() { IsSuccess = false, Message = message, Errors = errors.ToList() };
	}
}

namespace ThaiTuanERP2025.Api.Common
{
	public class ApiResponse<T>
	{
		public bool IsSuccess { get; set; }
		public string? Message { get; set; }
		public T? Data { get; set; } = default!;

		public static ApiResponse<T> Success(T data, string? message = null) => new() { IsSuccess = true, Data = data, Message = message };

		public static ApiResponse<T> Fail(string message) => new() { IsSuccess = false, Message = message };
	}
}

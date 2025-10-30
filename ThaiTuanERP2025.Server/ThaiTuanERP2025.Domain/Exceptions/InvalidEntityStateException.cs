namespace ThaiTuanERP2025.Domain.Exceptions
{
	/// <summary>
	/// Lỗi khi entity ở trạng thái không hợp lệ (ví dụ Role inactive mà gán Permission).
	/// </summary>
	public class InvalidEntityStateException : DomainException
	{
		public InvalidEntityStateException(string message) : base(message) { }
	}
}

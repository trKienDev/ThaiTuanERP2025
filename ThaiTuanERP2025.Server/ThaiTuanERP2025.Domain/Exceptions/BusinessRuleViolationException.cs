namespace ThaiTuanERP2025.Domain.Exceptions
{
	/// <summary>
	/// Lỗi khi vi phạm một quy tắc nghiệp vụ trong Domain.
	/// Ví dụ: "Role không thể bị xóa khi vẫn còn User được gán".
	/// </summary>
	public class BusinessRuleViolationException : DomainException
	{
		public BusinessRuleViolationException(string message) : base(message) { }
	}
}

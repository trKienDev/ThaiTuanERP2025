namespace ThaiTuanERP2025.Domain.Exceptions
{
	/// <summary>
	/// Exception dùng trong Domain Layer để biểu diễn lỗi nghiệp vụ.
	/// Không phụ thuộc HTTP hay framework ngoài.
	/// </summary>
	public class DomainException : Exception
	{
		public DomainException(string message) : base(message) { }
	}
}

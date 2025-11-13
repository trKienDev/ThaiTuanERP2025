namespace ThaiTuanERP2025.Application.Shared.Security
{
	public class PasswordHasher : IPasswordHasher
	{
		/// <summary>
		/// Băm mật khẩu thô sang dạng hash an toàn.
		/// </summary>
		public string Hash(string password) {
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		/// <summary>
		/// So sánh mật khẩu thô với hash đã lưu.
		/// </summary>
		public bool Verify(string password, string hashedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}
	}
}

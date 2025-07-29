using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Common.Security
{
	public class PasswordHasher
	{
		/// <summary>
		/// Băm mật khẩu thô sang dạng hash an toàn.
		/// </summary>
		public static string Hash(string password) {
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		/// <summary>
		/// So sánh mật khẩu thô với hash đã lưu.
		/// </summary>
		public static bool Verify(string password, string hashedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Department
	{
		public Guid Id { get;  set; }
		public string Name { get;  set; } = string.Empty;
		public string Code { get;  set; } = string.Empty;

		public ICollection<User> Users { get; private set; }

		private Department() {
			Users = new List<User>();
		} // EF
		public Department(string name, string code) {
			if(string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Tên phòng ban không được trống");
			if(string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException("Mã phòng ban không được trống");

			Id = Guid.NewGuid();
			Name = name.Trim();
			Code = code.ToUpperInvariant();
			Users = new List<User>();
		}

		public  void Rename(string newName) {
			if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentNullException("Tên mới không hợp lệ");
			Name = newName.Trim();
		}
	}
}

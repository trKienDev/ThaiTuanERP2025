using ThaiTuanERP2025.Domain.Account.Enums;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class Department : AuditableEntity
	{
		public string Name { get;  set; } = string.Empty;
		public string Code { get;  set; } = string.Empty;
		public bool IsActive { get; set; } = true;
		public int Level { get; set; }

		public ICollection<User> Users { get; private set; }
		
		public Guid? ParentId { get; set; }
		public Department? Parent { get; set; }
		public ICollection<Department> Children { get; private set; } = new List<Department>();	

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		public Region Region { get; set; } = Region.None;

		public Guid? ManagerUserId { get; set; }	
		public User? ManagerUser { get; set; }

		private Department() {
			Users = new List<User>();
		} // EF
		public Department(string name, string code, Region region, Guid? managerUserId = null) {
			if(string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Tên phòng ban không được trống");
			if(string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException("Mã phòng ban không được trống");

			Id = Guid.NewGuid();
			Name = name.Trim();
			Code = code.ToUpperInvariant();
			Region = region;
			ManagerUserId = managerUserId;
			Users = new List<User>();
		}

		public  void Rename(string newName) {
			if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentNullException("Tên mới không hợp lệ");
			Name = newName.Trim();
		}

		public void SetRegion(Region region) => Region = region;	
		public void SetManager(Guid? userId) => ManagerUserId = userId;
	}
}

using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Domain.Account.Enums;

namespace ThaiTuanERP2025.Infrastructure.Seeding
{
	public class DbInitializer
	{
		public static void Seed(ThaiTuanERP2025DbContext context)
		{
			if (!context.Users.Any(u => u.Username == "admin"))
			{
				// 1) Tạo admin CHƯA gán department
				var admin = new User(
				    fullName: "Admin",
				    userName: "admin",
				    employeeCode: "ITC01",
				    passwordHash: PasswordHasher.Hash("Th@iTu@n2025"),
				    role: UserRole.admin,
				    position: "System Admin",
				    departmentId: null,                                    // ✅
				    email: new Email("itcenter@thaituan.com.vn")
				);
				admin.SetSuperAdmin(true);
				context.Users.Add(admin);
				context.SaveChanges();

				// 2) Tạo phòng ban, gán CreatedByUserId = admin.Id
				var dept = new Department("Phòng IT", "ITC", Region.South);
				// nếu AuditableEntity có CreatedByUserId:
				// dept.CreatedByUserId = admin.Id;
				// hoặc: dept.CreatedByUser = admin;
				context.Departments.Add(dept);
				context.SaveChanges();

				// 3) Gán admin vào phòng ban vừa tạo
				admin.SetDepartment(dept.Id);
				context.SaveChanges();
			}
		}
	}
}

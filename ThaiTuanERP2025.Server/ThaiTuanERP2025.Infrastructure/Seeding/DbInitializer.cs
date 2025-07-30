using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				var dept = new Department("Phòng IT", "ITC");
				context.Departments.Add(dept);
				context.SaveChanges(); // phải lưu để có ID

				// sau khi đã có department
				if (!context.Users.Any())
				{
					var admin = new User(
						fullName: "Admin",
						userName: "admin",
						employeeCode: "ITC01",
						passwordHash: PasswordHasher.Hash("Th@iTu@n2025"),
						avatarUrl: "",
						role: UserRole.admin,
						position: "System Admin",
						departmentId: dept.Id,
						email: new Email("itcenter@thaituan.com.vn")
					);
					admin.SetSuperAdmin(true);

					context.Users.Add(admin);
					context.SaveChanges();
				}
			}
		}
	}
}

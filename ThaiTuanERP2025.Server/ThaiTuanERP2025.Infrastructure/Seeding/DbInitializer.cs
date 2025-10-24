using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Seeding
{
	public static class DbInitializer
	{
		public static async Task InitializeAsync(ThaiTuanERP2025DbContext context)
		{
			Console.WriteLine(">>> [DbInitializer] Seeding started...");

			// 1️⃣ Kiểm tra và chạy migration (nếu có)
			try
			{
				var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
				if (pendingMigrations.Any())
				{
					Console.WriteLine($">>> [DbInitializer] Applying {pendingMigrations.Count()} pending migrations...");
					await context.Database.MigrateAsync();
				}
				else
				{
					Console.WriteLine(">>> [DbInitializer] No pending migrations. Skip migration step.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("⚠️ [DbInitializer] Migration skipped. Reason: " + ex.Message);
			}

			// 2️⃣ Seed Department
			var departmentCount = await context.Departments.IgnoreQueryFilters().CountAsync();
			Console.WriteLine($">>> [DbInitializer] Department count: {departmentCount}");

			Department department;
			if (departmentCount == 0)
			{
				Console.WriteLine(">>> [DbInitializer] Creating default Department...");
				department = new Department("Phòng IT", "Công nghệ thông tin", Domain.Account.Enums.Region.None);
				context.Departments.Add(department);
				await context.SaveChangesAsync();
			}
			else
			{
				department = await context.Departments.IgnoreQueryFilters().FirstAsync();
				Console.WriteLine($">>> [DbInitializer] Found existing Department: {department.Name}");
			}

			// 3️⃣ Seed Admin User
			var userCount = await context.Users.IgnoreQueryFilters().CountAsync();
			Console.WriteLine($">>> [DbInitializer] User count: {userCount}");

			User adminUser;
			if (!await context.Users.IgnoreQueryFilters().AnyAsync(u => u.Username == "admin"))
			{
				Console.WriteLine(">>> [DbInitializer] Creating Admin User...");

				adminUser = new User(
				    fullName: "Admin",
				    userName: "admin",
				    employeeCode: "ITC01",
				    passwordHash: PasswordHasher.Hash("Th@iTu@n2025"),
				    position: "System Admin",
				    departmentId: department.Id,
				    email: new Email("itcenter@thaituan.com.vn")
				);

				context.Users.Add(adminUser);
				await context.SaveChangesAsync();
				Console.WriteLine(">>> [DbInitializer] Admin User created successfully.");
			}
			else
			{
				adminUser = await context.Users.IgnoreQueryFilters()
				    .FirstAsync(u => u.Username == "admin");
				Console.WriteLine(">>> [DbInitializer] Existing Admin User found.");
			}

			// 4️⃣ Gán role SuperAdmin cho user
			var superAdminRole = await context.Roles.IgnoreQueryFilters()
			    .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");

			if (superAdminRole == null)
			{
				Console.WriteLine("⚠️ [DbInitializer] SuperAdmin role not found! Make sure SeedDataExtensions.SeedRolesAndPermissions() is executed during OnModelCreating().");
			}
			else
			{
				bool hasUserRole = await context.UserRoles.IgnoreQueryFilters()
				    .AnyAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == superAdminRole.Id);

				if (!hasUserRole)
				{
					Console.WriteLine(">>> [DbInitializer] Assigning SuperAdmin role to admin...");
					var userRole = new UserRole(adminUser.Id, superAdminRole.Id);
					context.UserRoles.Add(userRole);
					await context.SaveChangesAsync();
					Console.WriteLine(">>> [DbInitializer] Admin assigned to SuperAdmin role successfully.");
				}
				else
				{
					Console.WriteLine(">>> [DbInitializer] Admin already has SuperAdmin role.");
				}
			}

			Console.WriteLine(">>> [DbInitializer] Seeding completed successfully.");
		}
	}
}

using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Seeding
{
	public sealed class DbInitializer
	{
		private readonly IPasswordHasher _passwordHasher;
		private readonly ThaiTuanERP2025DbContext _db;
		public DbInitializer(IPasswordHasher passwordHasher, ThaiTuanERP2025DbContext db) {
			_passwordHasher = passwordHasher;
			_db = db;
		}

		public async Task Seed()
		{
			Console.WriteLine(">>> [DbInitializer] Checking for admin user...");

			await _db.Database.MigrateAsync();

			if (!await _db.Roles.AnyAsync())
			{
				Console.WriteLine(">>> [DbInitializer] Creating default Roles...");

				var roles = new List<Role>
				{
					    new Role("SuperAdmin", "Toàn quyền hệ thống"),
				};

				await _db.Roles.AddRangeAsync(roles);
				await _db.SaveChangesAsync();
			}

			if (!await _db.Users.AnyAsync(u => u.Username == "admin"))
			{
				Console.WriteLine(">>> [DbInitializer] Creating Admin User...");

				var adminUser = new User(
				    fullName: "Admin",
				    userName: "admin",
				    employeeCode: "ADMIN",
				    passwordHash: _passwordHasher.Hash("Th@iTu@n2025"),
				    position: "System Admin",
				    departmentId: null,
				    email: new Email("itcenter@thaituan.com.vn")
				);

				await _db.Users.AddAsync(adminUser);
				await _db.SaveChangesAsync();

				// Gán quyền SuperAdmin
				var superAdminRole = await _db.Roles.FirstAsync(r => r.Name == "SuperAdmin");
				var adminUserRole = new UserRole(adminUser.Id, superAdminRole.Id);

				await _db.UserRoles.AddAsync(adminUserRole);
				await _db.SaveChangesAsync();

				Console.WriteLine(">>> [DbInitializer] Admin User created successfully.");
			}
			else
			{
				Console.WriteLine(">>> [DbInitializer] Admin User already exists, skipping creation.");
			}

			Console.WriteLine(">>> [DbInitializer] Seeding completed successfully!");
		}
	}
}

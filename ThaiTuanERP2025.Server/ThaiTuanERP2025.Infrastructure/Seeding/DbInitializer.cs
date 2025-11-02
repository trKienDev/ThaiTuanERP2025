using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Domain.Account.ValueObjects;

namespace ThaiTuanERP2025.Infrastructure.Seeding
{
	public sealed class DbInitializer
	{
		private readonly IPasswordHasher _passwordHasher;
		private readonly ThaiTuanERP2025DbContext _db;

		public DbInitializer(IPasswordHasher passwordHasher, ThaiTuanERP2025DbContext db)
		{
			_passwordHasher = passwordHasher;
			_db = db;
		}

		public async Task Seed()
		{
			Console.WriteLine(">>> [DbInitializer] Starting database initialization...");

			await _db.Database.MigrateAsync();

			// 1️ ) Tạo user Admin trước để có CreatedByUserId
			var adminUser = await _db.Users.FirstOrDefaultAsync(u => u.Username == "admin");
			if (adminUser == null)
			{
				Console.WriteLine(">>> [DbInitializer] Creating Admin User...");

				adminUser = new User(
					fullName: "Admin",
					username: "admin",
					employeeCode: "ADMIN",
					passwordHash: _passwordHasher.Hash("Th@iTu@n2025"),
					position: "System Admin",
					departmentId: null,
					email: new Email("itcenter@thaituan.com.vn")
				);

				await _db.Users.AddAsync(adminUser);
				await _db.SaveChangesAsync();

				Console.WriteLine(">>> [DbInitializer] Admin User created successfully.");
			}
			else
			{
				Console.WriteLine(">>> [DbInitializer] Admin User already exists.");
			}

			// 2️ ) Sau khi có adminUser.Id → tạo Role với CreatedByUserId = adminUser.Id
			if (!await _db.Roles.AnyAsync())
			{
				Console.WriteLine(">>> [DbInitializer] Creating default Roles...");

				var roles = new List<Role>
				{
					new Role("SuperAdmin", "Toàn quyền hệ thống")
				};

				await _db.Roles.AddRangeAsync(roles);
				await _db.SaveChangesAsync();

				Console.WriteLine(">>> [DbInitializer] Default roles created successfully.");
			}
			else
			{
				Console.WriteLine(">>> [DbInitializer] Roles already exist, skipping creation.");
			}


			// 3️ ) Gán quyền SuperAdmin cho user admin
			if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == adminUser.Id))
			{
				Console.WriteLine(">>> [DbInitializer] Assigning SuperAdmin role to admin user...");

				var superAdminRole = await _db.Roles.FirstAsync(r => r.Name == "SuperAdmin");
				adminUser.AssignRole(superAdminRole.Id);

				await _db.SaveChangesAsync();

				Console.WriteLine(">>> [DbInitializer] SuperAdmin role assigned to admin user successfully.");
			}

			Console.WriteLine(">>> [DbInitializer] ✅ Seeding completed successfully!");
		}
	}
}

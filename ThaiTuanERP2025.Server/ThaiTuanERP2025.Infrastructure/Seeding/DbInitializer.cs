using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Seeding
{
	public class DbInitializer
	{
		private readonly IPasswordHasher _passwordHasher;
		public DbInitializer(IPasswordHasher passwordHasher) {
			_passwordHasher = passwordHasher;
		}

		public async Task InitializeAsync(ThaiTuanERP2025DbContext context)
		{
			Console.WriteLine(">>> [DbInitializer] Checking for admin user...");

			await context.Database.MigrateAsync();

			var existingAdmin = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");

			if (existingAdmin != null)
			{
				Console.WriteLine(">>> [DbInitializer] Admin user already exists.");
				return;
			}

			Console.WriteLine(">>> [DbInitializer] Creating new admin user...");

			var admin = new User(
				fullName: "Administrator",
				userName: "admin",
				employeeCode: "ADMIN001",
				passwordHash: _passwordHasher.Hash("Th@iTu@n2025"),
				position: "System Admin",
				departmentId: null,
				email: new Email("admin@thaituan.com.vn"),
				phone: new Phone("0900000000")
			);

			admin.SetSuperAdmin(true);

			context.Users.Add(admin);
			await context.SaveChangesAsync();

			Console.WriteLine(">>> [DbInitializer] Admin user created successfully!");
			Console.WriteLine(">>> Username: admin");
			Console.WriteLine(">>> Password: Th@iTu@n2025");
		}
	}
}

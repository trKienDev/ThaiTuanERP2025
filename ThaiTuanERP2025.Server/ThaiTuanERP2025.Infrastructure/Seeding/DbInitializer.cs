using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Application.Shared.Security;
using ThaiTuanERP2025.Infrastructure.Persistence;

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
			await MigrateAsync();

			var adminUser = await EnsureAdminUserAsync();
			var superAdminRole = await EnsureDefaultRolesAsync();

			await AssignSuperAdminToAdminAsync(adminUser, superAdminRole);
			await SeedPermissionsAsync();
			// await GrantAllPermissionsToSuperAdminAsync(); // nếu muốn

			Console.WriteLine(">>> [DbInitializer] ✅ Seeding completed successfully!");
		}

		private async Task MigrateAsync() => await _db.Database.MigrateAsync();

		private async Task<User> EnsureAdminUserAsync()
		{
			var admin = await _db.Users.FirstOrDefaultAsync(u => u.Username == "admin");
			if (admin != null) return admin;

			admin = new User(
			    fullName: "Admin",
			    username: "admin",
			    employeeCode: "ADMIN",
			    passwordHash: _passwordHasher.Hash("Th@iTu@n2025"),
			    position: "System Admin",
			    departmentId: null,
			    email: "itcenter@thaituan.com.vn"
			);
			await _db.Users.AddAsync(admin);
			await _db.SaveChangesAsync();
			return admin;
		}

		private async Task<Role> EnsureDefaultRolesAsync()
		{
			var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "SuperAdmin");
			if (role != null) return role;

			role = new Role("SuperAdmin", "Toàn quyền hệ thống");
			await _db.Roles.AddAsync(role);
			await _db.SaveChangesAsync();
			return role;
		}

		private async Task AssignSuperAdminToAdminAsync(User admin, Role superAdmin)
		{
			if (!await _db.UserRoles.AnyAsync(ur => ur.UserId == admin.Id && ur.RoleId == superAdmin.Id))
			{
				admin.AssignRole(superAdmin.Id);
				await _db.SaveChangesAsync();
			}
		}

		private async Task SeedPermissionsAsync()
		{
			var desired = new (string Name, string Code, string? Description)[]
			{
				("Xóa user", "user.delete", null),
				("Tạo kỳ ngân sách", "budget-period.create-for-year", null),
				("Tạo phòng ban", "department.create", null),
				("Thao tác phòng ban", "department.actions", null),
				("Tạo người dùng", "user.create", null),
				("Thao tác user", "user.actions", null),
				("Tạo nhóm ngân sách", "budget-group.create", null),
				("Cập nhật kỳ ngân sách", "budget-period.edit", null),
				("Thêm user duyệt ngân sách", "budget-approver.create", null),
				("Thao tác user duyệt ngân sách", "budget-approver.actions", null)
			};

			foreach (var (name, code, desc) in desired)
			{
				var codeNorm = code.Trim().ToLowerInvariant();
				var existing = await _db.Permissions.FirstOrDefaultAsync(p => p.Code == codeNorm);
				if (existing == null)
				{
					await _db.Permissions.AddAsync(new Permission(name, codeNorm, desc ?? string.Empty));
				}
				else
				{
					var nameTrim = name.Trim();
					var newDesc = (desc ?? string.Empty).Trim();

					if (!string.Equals(existing.Name, nameTrim, StringComparison.Ordinal))
						existing.Rename(nameTrim);

					if (!string.Equals(existing.Description, newDesc, StringComparison.Ordinal))
						existing.UpdateDescription(newDesc);

					if (!existing.IsActive)
						existing.Activate();
				}
			}
			await _db.SaveChangesAsync();
		}
	}
}

using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Persistence.Seeds
{
	public static class SeedDataExtensions
	{
		public static void SeedRolesAndPermissions(this ModelBuilder modelBuilder)
		{
			var superAdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");

			// KHÔNG dùng new Role(...), dùng anonymous object với giá trị tĩnh
			modelBuilder.Entity<Role>().HasData(new
			{
				Id = superAdminRoleId,
				Name = "SuperAdmin",
				Description = "Toàn quyền hệ thống",

				// các cột auditing bắt buộc (nếu NOT NULL trong migration) dùng giá trị cố định
				CreatedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc),
				CreatedByUserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
				IsDeleted = false
			});
		}
	}
}

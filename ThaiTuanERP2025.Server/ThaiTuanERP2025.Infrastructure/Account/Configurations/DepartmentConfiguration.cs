using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
	{
		public void Configure(EntityTypeBuilder<Department> builder)
		{
			builder.ToTable("Departments", "Account");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Code).IsRequired().HasMaxLength(64);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Region).HasConversion<int>().IsRequired();
			builder.Property(x => x.Level).IsRequired();

			builder.HasOne(x => x.Parent)
				.WithMany(x => x.Children)
				.HasForeignKey(x => x.ParentId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(x => x.Code).IsUnique();
			builder.HasIndex(x => x.Name);

			builder.HasOne(d => d.ManagerUser)
				.WithMany()                
				.HasForeignKey(d => d.ManagerUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.CreatedByUser)
				.WithMany()
				.HasForeignKey(e => e.CreatedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.ModifiedByUser)
				.WithMany()
				.HasForeignKey(e => e.ModifiedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.DeletedByUser)
				.WithMany()
				.HasForeignKey(e => e.DeletedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => e.CreatedByUserId);
			builder.HasIndex(e => e.ModifiedByUserId);
			builder.HasIndex(e => e.DeletedByUserId);
		}
	}
}

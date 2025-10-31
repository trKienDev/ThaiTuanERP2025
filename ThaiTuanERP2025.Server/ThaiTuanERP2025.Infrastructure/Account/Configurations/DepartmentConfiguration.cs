﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class DepartmentConfiguration : BaseEntityConfiguration<Department>
	{
		public override void Configure(EntityTypeBuilder<Department> builder)
		{
			builder.ToTable("Departments", "Account");

			builder.HasKey(d => d.Id);

			builder.Property(d => d.Name).IsRequired().HasMaxLength(150);
			builder.Property(d => d.Code).IsRequired().HasMaxLength(50);
			builder.Property(d => d.Level).HasDefaultValue(0);
			builder.Property(d => d.IsActive).HasDefaultValue(true);

			// Self reference (Parent)
			builder.HasOne(d => d.Parent)
				.WithMany(p => p.Children)
				.HasForeignKey(d => d.ParentId)
				.OnDelete(DeleteBehavior.Restrict);

			// Manager (User)
			builder.HasOne(d => d.ManagerUser)
				.WithMany()
				.HasForeignKey(d => d.ManagerUserId)
				.OnDelete(DeleteBehavior.SetNull);

			// Indexes
			builder.HasIndex(d => d.Code).IsUnique();

			ConfigureAuditUsers(builder);
		}
	}
}
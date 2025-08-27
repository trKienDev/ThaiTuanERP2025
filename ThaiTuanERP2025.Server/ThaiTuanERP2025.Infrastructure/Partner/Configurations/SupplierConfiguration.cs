using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Partner.Entities;

namespace ThaiTuanERP2025.Infrastructure.Partner.Configurations
{
	public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
	{
		public void Configure(EntityTypeBuilder<Supplier> builder)
		{
			builder.ToTable("Supplier", "Partner");

			builder.HasKey(e => e.Id);

			builder.HasIndex(e => e.Code).IsUnique().HasFilter("[IsDeleted] = 0"); // unique Code (không đụng bản ghi đã xóa mềm)
			builder.Property(e => e.Code).IsRequired().HasMaxLength(30);
			builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
			builder.Property(e => e.ShortName).HasMaxLength(50);
			builder.Property(e => e.IsActive).HasDefaultValue(true);

			builder.Property(e => e.TaxCode).HasMaxLength(50);
			builder.Property(e => e.WithholdingTaxType).HasMaxLength(20);
			builder.Property(e => e.WithholdingTaxRate).HasPrecision(5, 2);
			builder.Property(e => e.DefaultCurrency).IsRequired().HasMaxLength(3);
			builder.Property(e => e.PaymentTermDays).HasDefaultValue(30);

			builder.Property(e => e.Email).HasMaxLength(150);
			builder.Property(e => e.Phone).HasMaxLength(30);
			builder.Property(e => e.AddressLine1).HasMaxLength(200);
			builder.Property(e => e.AddressLine2).HasMaxLength(200);
			builder.Property(e => e.City).HasMaxLength(100);
			builder.Property(e => e.StateOrProvince).HasMaxLength(100);
			builder.Property(e => e.PostalCode).HasMaxLength(20);
			builder.Property(e => e.Country).HasMaxLength(2);
			builder.Property(e => e.Note).HasMaxLength(500);

			// Quan hệ Audit users (giống các entity khác của bạn)
			builder.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(e => e.CreatedByUserId);
			builder.HasOne(e => e.ModifiedByUser).WithMany().HasForeignKey(e => e.ModifiedByUserId).OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(e => e.ModifiedByUserId);
			builder.HasOne(e => e.DeletedByUser).WithMany().HasForeignKey(e => e.DeletedByUserId).OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(e => e.DeletedByUserId);

			builder.ToTable(t => t.HasCheckConstraint(
				 "CK_Suppliers_PaymentTermDays",
				"[PaymentTermDays] IS NULL OR ([PaymentTermDays] BETWEEN 0 AND 365)"
			));
		}
	}
}

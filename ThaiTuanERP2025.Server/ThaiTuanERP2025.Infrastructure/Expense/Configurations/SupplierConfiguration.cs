using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public sealed class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
	{
		public void Configure(EntityTypeBuilder<Supplier> builder)
		{
			builder.ToTable("Suppliers", "Expense");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
			builder.Property(x => x.TaxCode).IsRequired().HasMaxLength(32);
			builder.Property(x => x.IsActive).HasDefaultValue(true);

			builder.HasIndex(x => x.Name);
			builder.HasIndex(x => x.TaxCode);
		}
	}
}

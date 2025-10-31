using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public sealed class SupplierConfiguration : BaseEntityConfiguration<Supplier>
	{
		public override void Configure(EntityTypeBuilder<Supplier> builder)
		{
			builder.ToTable("Suppliers", "Expense");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
			builder.Property(x => x.TaxCode).HasMaxLength(50);
			builder.Property(x => x.IsActive).IsRequired();

			// ========== Navigation tới BankAccounts ==========
			builder.Navigation(nameof(Supplier.BankAccounts)).UsePropertyAccessMode(PropertyAccessMode.Field);
			// EF truy cập trực tiếp private field _bankAccounts

			builder.HasMany(typeof(BankAccount), "_bankAccounts")
				.WithOne(nameof(BankAccount.Supplier))
				.HasForeignKey(nameof(BankAccount.SupplierId))
				.OnDelete(DeleteBehavior.Cascade);
			// Khi xóa Supplier thì xóa luôn BankAccounts (trong Aggregate)

			// ========== Index ==========
			builder.HasIndex(x => x.Name);
			builder.HasIndex(x => x.TaxCode);
		}
	}
}

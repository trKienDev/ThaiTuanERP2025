using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public sealed class BankAccountConfiguration : BaseEntityConfiguration<BankAccount>
	{
		public override void Configure(EntityTypeBuilder<BankAccount> builder)
		{
			builder.ToTable("BankAccounts", "Expense");

			builder.HasKey(x => x.Id);

			// ========== Thuộc tính cơ bản ==========
			builder.Property(x => x.BankName).HasMaxLength(200).IsRequired();
			builder.Property(x => x.AccountNumber).HasMaxLength(100).IsRequired();
			builder.Property(x => x.BeneficiaryName).HasMaxLength(200).IsRequired();
			builder.Property(x => x.IsActive).IsRequired();

			// ========== Relationship User ==========
			builder.HasOne(x => x.User)
				.WithMany(u => u.BankAccounts)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			// ========== Index ==========
			builder.HasIndex(x => x.AccountNumber).IsUnique(); // tránh trùng số tài khoản
			builder.HasIndex(x => new { x.SupplierId, x.IsActive });
			builder.HasIndex(x => new { x.UserId, x.IsActive });

			ConfigureAuditUsers(builder);
		}
	}
}

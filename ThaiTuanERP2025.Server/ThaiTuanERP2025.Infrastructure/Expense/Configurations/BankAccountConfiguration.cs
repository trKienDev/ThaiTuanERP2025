using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public sealed class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
	{
		public void Configure(EntityTypeBuilder<BankAccount> builder) {
			builder.ToTable("BankAccounts", "Expense", t => {
				// CHECK: đúng 1 trong 2 FK(UserId, SupplierId) được set
				t.HasCheckConstraint(
					"CK_Finance_BankAccount_Owner",
					"(CASE WHEN [UserId] IS NOT NULL THEN 1 ELSE 0 END) + " +
					"(CASE WHEN [SupplierId] IS NOT NULL THEN 1 ELSE 0 END) = 1"
				);
			});
			builder.HasKey(x => x.Id);

			builder.Property(x => x.BankName).IsRequired().HasMaxLength(128);
			builder.Property(x => x.AccountNumber).IsRequired().HasMaxLength(64);
			builder.Property(x => x.BeneficiaryName).IsRequired().HasMaxLength(128);

			// Owner: hoặc User hoặc Supplier
			builder.HasOne(x => x.User)
				.WithMany(u => u.BankAccounts)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.Supplier)
				.WithMany(s => s.BankAccounts)
				.HasForeignKey(x => x.SupplierId)
				.OnDelete(DeleteBehavior.Restrict);

			// Mỗi User không được lặp AccountNumber
			builder.HasIndex(x => new { x.UserId, x.AccountNumber })
				.IsUnique()
				.HasFilter("[UserId] IS NOT NULL");

			// Mỗi Supplier không được lặp AccountNumber
			builder.HasIndex(x => new { x.SupplierId, x.AccountNumber })
				.IsUnique()
				.HasFilter("[SupplierId] IS NOT NULL");
		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public sealed class OutgoingBankAcccountConfiguration : IEntityTypeConfiguration<OutgoingBankAccount>
	{
		public void Configure(EntityTypeBuilder<OutgoingBankAccount> builder)
		{
			builder.ToTable("OutgoingBankAccounts", "Expense");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
			builder.Property(x => x.BankName).IsRequired().HasMaxLength(128);
			builder.Property(x => x.AccountNumber).IsRequired().HasMaxLength(64);
			builder.Property(x => x.OwnerName).IsRequired().HasMaxLength(128);
			builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);

			builder.HasIndex(x => new { x.Name, x.AccountNumber, x.BankName }).IsUnique()
				.HasDatabaseName("UX_OutgoingBankAccount_Account_Bank");

			builder.HasIndex(x => x.IsActive)
			       .HasDatabaseName("IX_OutgoingBankAccount_IsActive");
		}
	}
}

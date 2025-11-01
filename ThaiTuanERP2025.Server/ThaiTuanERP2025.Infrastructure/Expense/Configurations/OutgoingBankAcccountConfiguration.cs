using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public sealed class OutgoingBankAcccountConfiguration : BaseEntityConfiguration<OutgoingBankAccount>
	{
		public override void Configure(EntityTypeBuilder<OutgoingBankAccount> builder)
		{
			builder.ToTable("OutgoingBankAccounts", "Expense");
			builder.HasKey(x => x.Id);

			// ========== Thuộc tính cơ bản ==========
			builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
			builder.Property(x => x.BankName).HasMaxLength(200).IsRequired();
			builder.Property(x => x.AccountNumber).HasMaxLength(100).IsRequired();
			builder.Property(x => x.OwnerName).HasMaxLength(200).IsRequired();
			builder.Property(x => x.IsActive).IsRequired();

			// ========== Index ==========
			// Đảm bảo một AccountNumber duy nhất trong hệ thống
			builder.HasIndex(x => x.AccountNumber).IsUnique();

			// Tối ưu tìm kiếm theo trạng thái và tên
			builder.HasIndex(x => new { x.IsActive, x.Name });

			// Auditable	
			ConfigureAuditUsers(builder);
		}
	}
}

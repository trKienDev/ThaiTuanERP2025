using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class InvoiceFileConfiguration : IEntityTypeConfiguration<InvoiceFile>
	{
		public void Configure(EntityTypeBuilder<InvoiceFile> builder)
		{
			builder.ToTable("InvoiceFiles", "Expense");
			builder.HasKey(f => f.Id);

			builder.Property(x => x.IsMain).HasDefaultValue(false);
			builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

			builder.HasOne(f => f.File)
				.WithMany()
				.HasForeignKey(f => f.FileId)
				.OnDelete(DeleteBehavior.Restrict);

			// mỗi invoice chỉ có 12 file IsMain = 1
			builder.HasIndex(f => f.InvoiceId)
				.HasDatabaseName("UX_InvoiceFiles_Main")
				.HasFilter("[IsMain] = 1")
				.IsUnique();
		}
	}
}

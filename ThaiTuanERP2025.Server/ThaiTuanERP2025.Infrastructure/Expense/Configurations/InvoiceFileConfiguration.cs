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

			builder.Property(x => x.IsMain).IsRequired();
			builder.Property(x => x.CreatedAt).IsRequired();

			builder.HasOne(x => x.File)
				.WithMany()
				.HasForeignKey(x => x.FileId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(x => new { x.InvoiceId, x.IsMain }).HasFilter("[IsMain] = 1").IsUnique();
			builder.HasIndex(x => x.CreatedAt);
		}
	}
}

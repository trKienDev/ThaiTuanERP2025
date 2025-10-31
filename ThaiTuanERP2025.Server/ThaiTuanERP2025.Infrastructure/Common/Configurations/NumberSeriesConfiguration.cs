using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Common.Entities;

namespace ThaiTuanERP2025.Infrastructure.Common.Configurations
{
	public class NumberSeriesConfiguration : IEntityTypeConfiguration<NumberSeries>
	{
		public void Configure(EntityTypeBuilder<NumberSeries> builder)
		{
			builder.ToTable("NumberSeries");
			builder.HasKey(x => x.Id);

			builder.HasIndex(x => x.Key).IsUnique();
			builder.Property(x => x.Key).IsRequired().HasMaxLength(100);

			builder.Property(x => x.Prefix).IsRequired().HasMaxLength(20);
			builder.Property(x => x.PadLength).IsRequired().HasDefaultValue(6); // độ dài padding, mặc định 6 ký tự
			builder.Property(x => x.NextNumber).IsRequired();

			builder.Property(x => x.RowVersion).IsRowVersion(); // concurrency token
		}
	}
}

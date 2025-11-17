using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
	{
		public void Configure(EntityTypeBuilder<OutboxMessage> builder)
		{
			builder.ToTable("OutboxMessages", "Core");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Type).IsRequired().HasMaxLength(100);
			builder.Property(x => x.Payload).IsRequired();
			builder.Property(x => x.OccurredOnUtc).IsRequired();
			builder.Property(x => x.RetryCount).HasDefaultValue(0);
		}
	}
}

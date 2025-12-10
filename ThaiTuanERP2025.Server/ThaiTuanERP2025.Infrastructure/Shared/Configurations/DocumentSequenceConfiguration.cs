using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Shared.Entities;

namespace ThaiTuanERP2025.Infrastructure.Shared.Configurations
{
	public class DocumentSequenceConfiguration : IEntityTypeConfiguration<DocumentSequence>
	{	
		public void Configure(EntityTypeBuilder<DocumentSequence> builder) {
			builder.ToTable("DocumentSequences");
			builder.HasKey(x => x.Key);
			builder.Property(x => x.Key).HasMaxLength(100).IsRequired();
			builder.Property(x => x.LastNumber).IsRequired();
		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Common.Entities;

namespace ThaiTuanERP2025.Infrastructure.Common.Configurations
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

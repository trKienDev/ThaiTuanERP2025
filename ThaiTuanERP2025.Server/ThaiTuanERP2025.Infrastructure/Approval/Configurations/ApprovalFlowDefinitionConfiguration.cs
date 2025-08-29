using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Approval.Configurations
{
	public class ApprovalFlowDefinitionConfiguration : IEntityTypeConfiguration<ApprovalFlowDefinition>
	{
		public void Configure(EntityTypeBuilder<ApprovalFlowDefinition> builder)
		{
			builder.ToTable("FlowDefinitions", "Approval");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
			builder.Property(x => x.DocumentType).HasMaxLength(100).IsRequired();
			builder.Property(x => x.Version).IsRequired();

			builder.Property(x => x.RowVersion).IsRowVersion();

			builder.HasMany(x => x.Steps)
				.WithOne(x => x.FlowDefinition)
				.HasForeignKey(x => x.FlowDefinitionId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(x => new { x.DocumentType, x.Version });
			builder.HasIndex(x => x.IsActive);
		}
	}
}

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
	public class ApprovalFlowInstanceConfiguration : IEntityTypeConfiguration<ApprovalFlowInstance>
	{
		public void Configure(EntityTypeBuilder<ApprovalFlowInstance> builder)
		{
			builder.ToTable("FlowInstances", "Approval");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.DocumentType).HasMaxLength(100).IsRequired();
			builder.Property(x => x.Status).HasConversion<int>().IsRequired();
			builder.Property(x => x.RowVersion).IsRowVersion();

			builder.HasMany(x => x.Steps)
				.WithOne(x => x.FlowInstance)
				.HasForeignKey(x => x.FlowInstanceId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(x => new { x.DocumentType, x.DocumentId }).IsUnique();
		}

	}
}

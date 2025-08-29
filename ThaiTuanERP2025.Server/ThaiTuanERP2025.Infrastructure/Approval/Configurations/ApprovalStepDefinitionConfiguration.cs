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
	public class ApprovalStepDefinitionConfiguration : IEntityTypeConfiguration<ApprovalStepDefinition>
	{
		public void Configure(EntityTypeBuilder<ApprovalStepDefinition> builder)
		{
			builder.ToTable("StepDefinitions", "Approval");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
			builder.Property(x => x.ResolverParamsJson).HasColumnType("nvarchar(max)").IsRequired();

			builder.Property(x => x.Mode).HasConversion<int>().IsRequired();
			builder.Property(x => x.ResolverType).HasConversion<int>().IsRequired();

			builder.Property(x => x.RowVersion).IsRowVersion();
			builder.Property(x => x.RequiredCount).HasDefaultValue(1);

			builder.HasIndex(x => new { x.FlowDefinitionId, x.OrderIndex });

			// Check constraint: RequiredCount >= 1
			builder.ToTable(t => t.HasCheckConstraint("CK_ApprovalStepDefinition_RequiredCount", "[RequiredCount] >= 1"));
		}
	}
}

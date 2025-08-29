using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Approval.Configurations
{
	public class ApprovalStepInstanceConfiguration : IEntityTypeConfiguration<ApprovalStepInstance>
	{
		public void Configure(EntityTypeBuilder<ApprovalStepInstance> builder)
		{
			builder.ToTable("StepInstances", "Approval");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).HasMaxLength(256).IsRequired();
			builder.Property(x => x.Status).HasConversion<int>().IsRequired();
			
			builder.Property(x => x.CandidatesJson).HasColumnType("nvarchar(max)").IsRequired();
			builder.Property(x => x.RowVersion).IsRowVersion();

			builder.HasIndex(x => new { x.FlowInstanceId, x.OrderIndex });

			builder.HasOne(x => x.StepDefinition)
				.WithMany()
				.HasForeignKey(x => x.StepDefinitionId)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}

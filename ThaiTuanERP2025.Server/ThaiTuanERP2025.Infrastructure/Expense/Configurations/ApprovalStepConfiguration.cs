using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
	{
		public void Configure(EntityTypeBuilder<ApprovalStep> builder)
		{
			builder.ToTable("ApprovalSteps");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
			builder.Property(x => x.SlaHours).IsRequired();
			builder.Property(x => x.FlowType).HasConversion<int>().IsRequired();
			builder.Property(x => x.Order).IsRequired();

			// JSON converter cho ApproverIds
			builder.Property(x => x.ApproverIds)
				   .HasConversion(
					   v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
					   v => System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<Guid>())
				   .HasColumnType("nvarchar(max)")
				   .IsRequired();

			// shadow property for WorkflowId
			builder.Property<Guid>("WorkflowId").IsRequired();

			builder.HasIndex("WorkflowId", nameof(ApprovalStep.Order));
		}
	}
}

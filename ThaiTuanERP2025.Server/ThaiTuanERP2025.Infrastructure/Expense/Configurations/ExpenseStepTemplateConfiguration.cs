using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ExpenseStepTemplateConfiguration : IEntityTypeConfiguration<ExpenseStepTemplate>
	{
		public void Configure(EntityTypeBuilder<ExpenseStepTemplate> builder)
		{
			builder.ToTable("ExpenseStepTemplates", "ExpenseWorkflow");
			builder.HasKey(x => x.Id);

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
			builder.Property(x => x.Order).IsRequired();
			builder.Property(x => x.FlowType).HasConversion<int>().IsRequired();
			builder.Property(x => x.ApproverMode).HasConversion<int>().IsRequired();
			builder.Property(x => x.SlaHours).IsRequired();
			builder.Property(x => x.FixedApproverIdsJson).HasColumnType("nvarchar(max)");
			builder.Property(x => x.ResolverKey).HasMaxLength(100);
			builder.Property(x => x.ResolverParamsJson).HasColumnType("nvarchar(max)");
			builder.Property(x => x.AllowOverride).IsRequired();
	
			builder.HasIndex(x => new { x.WorkflowTemplateId, x.Order }).IsUnique();
		}
	}
}

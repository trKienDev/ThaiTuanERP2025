using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ApprovalWorkflowConfiguration : IEntityTypeConfiguration<ApprovalWorkflow>
	{
		public void Configure(EntityTypeBuilder<ApprovalWorkflow> builder)
		{
			builder.ToTable("ApprovalWorkflows");
			builder.HasKey(x => x.Id);
			
			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

			builder.HasMany(w => w.Steps)
			       .WithOne()
			       .HasForeignKey("WorkflowId")
			       .OnDelete(DeleteBehavior.Cascade);
		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ApprovalWorkflowConfiguration : IEntityTypeConfiguration<ApprovalWorkflow>
	{
		public void Configure(EntityTypeBuilder<ApprovalWorkflow> builder) {
			builder.ToTable("ApprovalWorkflows", "Expense");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
			builder.Property(x => x.IsActive).HasDefaultValue(true);

			builder.HasIndex(x => x.IsActive);

			// map theo navigation 'Steps'
			builder.HasMany(x => x.Steps)
				   .WithOne(x => x.ApprovalWorkflow)
				   .HasForeignKey(x => x.ApprovalWorkflowId)
				   .OnDelete(DeleteBehavior.Cascade);

			// backing field '_steps' _ Field access
			var nav = builder.Metadata.FindNavigation(nameof(ApprovalWorkflow.Steps))!;
			nav.SetField("_steps");
			nav.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}

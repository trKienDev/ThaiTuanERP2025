using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ExpenseWorkflowInstanceConfiguration : IEntityTypeConfiguration<ExpenseWorkflowInstance>
	{
		public void Configure(EntityTypeBuilder<ExpenseWorkflowInstance> builder)
		{
			builder.ToTable("ExpesneWorkflowInstance", "ExpenseWorkflow");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.DocumentType).HasMaxLength(100).IsRequired();
			builder.Property(x => x.Status).HasConversion<int>().IsRequired();
			builder.Property(x => x.TemplateVersion).IsRequired();
			builder.Property(x => x.RawJson).HasColumnType("nvarchar(max)");
			builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
			builder.Property(x => x.Currency).HasMaxLength(10);
			builder.Property(x => x.BudgetCode).HasMaxLength(50);
			builder.Property(x => x.CostCenter).HasMaxLength(50);

			builder.HasMany(x => x.Steps)
				.WithOne(x => x.WorkflowInstance)
				.HasForeignKey(x => x.WorkflowInstanceId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(x => new { x.DocumentType, x.DocumentId });
		}
	}
}

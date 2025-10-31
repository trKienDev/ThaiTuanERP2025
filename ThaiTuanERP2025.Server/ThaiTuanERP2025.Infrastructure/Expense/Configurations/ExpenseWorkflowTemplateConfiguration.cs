using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	//public class ExpenseWorkflowTemplateConfiguration : IEntityTypeConfiguration<ExpenseWorkflowTemplate>
	//{
	//	public void Configure(EntityTypeBuilder<ExpenseWorkflowTemplate> builder) {
	//		builder.ToTable("ExpenseWorkflowTemplates", "ExpenseWorkflow");
	//		builder.HasKey(x => x.Id);

	//		builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
	//		builder.Property(x => x.DocumentType).IsRequired().HasMaxLength(100);
	//		builder.Property(x => x.Version).IsRequired();
	//		builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
	//		builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

	//		builder.HasMany(x => x.Steps)
	//		       .WithOne(s => s.WorkflowTemplate)
	//		       .HasForeignKey(s => s.WorkflowTemplateId)
	//		       .OnDelete(DeleteBehavior.Cascade);

	//		builder.HasIndex(x => new { x.DocumentType, x.Version });
	//	}
	//}
}

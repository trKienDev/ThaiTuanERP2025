using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ApprovalWorkflowInstanceConfiguration : IEntityTypeConfiguration<ApprovalWorkflowInstance>
	{
		public void Configure(EntityTypeBuilder<ApprovalWorkflowInstance> b)
		{
			b.ToTable("ApprovalWorkflowInstance", "Workflow");
			b.HasKey(x => x.Id);

			b.Property(x => x.DocumentType).IsRequired().HasMaxLength(100);
			b.Property(x => x.Currency).HasMaxLength(3);
			b.Property(x => x.RawJson).HasColumnType("NVARCHAR(MAX)");

			b.Property(x => x.Status).HasConversion<byte>().IsRequired();

			b.HasIndex(x => new { x.DocumentType, x.DocumentId }).HasDatabaseName("IX_AWI_DocType_DocId");
			b.HasIndex(x => new { x.Status, x.CurrentStepOrder }).HasDatabaseName("IX_AWI_Status_CurrentStep");
			b.HasIndex(x => x.BudgetCode).HasDatabaseName("IX_AWI_BudgetCode");
			b.HasIndex(x => x.Amount).HasDatabaseName("IX_AWI_Amount");

			b.ToTable(t =>
			{
				t.HasCheckConstraint("CK_AWI_RawJson_JSON", "([RawJson] IS NULL OR ISJSON([RawJson]) = 1)");
			});

			b.HasMany(x => x.Steps)
			 .WithOne(s => s.WorkflowInstance)
			 .HasForeignKey(s => s.WorkflowInstanceId)
			 .OnDelete(DeleteBehavior.Cascade);
		}
	}
}

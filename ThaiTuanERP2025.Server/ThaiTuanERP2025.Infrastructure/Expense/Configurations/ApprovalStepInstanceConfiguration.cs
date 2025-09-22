using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ApprovalStepInstanceConfiguration : IEntityTypeConfiguration<ApprovalStepInstance>
	{
		public void Configure(EntityTypeBuilder<ApprovalStepInstance> b)
		{
			b.ToTable("ApprovalStepInstance", "Workflow");
			b.HasKey(x => x.Id);

			b.Property(x => x.Name).IsRequired().HasMaxLength(200);
			b.Property(x => x.FlowType).HasConversion<byte>().IsRequired();
			b.Property(x => x.ApproverMode).HasConversion<byte>().IsRequired();
			b.Property(x => x.Status).HasConversion<byte>().IsRequired();

			b.Property(x => x.ResolvedApproverCandidatesJson)
			    .HasColumnName("ResolvedApproverCandidates")
			    .HasColumnType("NVARCHAR(MAX)");

			b.Property(x => x.HistoryJson)
			    .HasColumnType("NVARCHAR(MAX)");

			b.HasIndex(x => new { x.WorkflowInstanceId, x.Order })
			 .IsUnique()
			 .HasDatabaseName("UX_ASI_Workflow_Order");

			b.HasIndex(x => new { x.Status, x.DueAt }).HasDatabaseName("IX_ASI_Status_DueAt");
			b.HasIndex(x => x.SelectedApproverId).HasDatabaseName("IX_ASI_SelectedApprover");
			b.HasIndex(x => new { x.ApprovedBy, x.ApprovedAt }).HasDatabaseName("IX_ASI_ApprovedBy_ApprovedAt");

			b.ToTable(t =>
			{
				t.HasCheckConstraint("CK_ASI_ResolvedCandidates_JSON",
				    "([ResolvedApproverCandidates] IS NULL OR ISJSON([ResolvedApproverCandidates]) = 1)");
				t.HasCheckConstraint("CK_ASI_History_JSON",
				    "([HistoryJson] IS NULL OR ISJSON([HistoryJson]) = 1)");
			});
		}
	}
}

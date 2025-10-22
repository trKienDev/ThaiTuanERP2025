using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ApprovalStepInstanceConfiguration : IEntityTypeConfiguration<ApprovalStepInstance>
	{
		public void Configure(EntityTypeBuilder<ApprovalStepInstance> builder)
		{
			builder.ToTable("ApprovalStepInstance", "Workflow");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.FlowType).HasConversion<byte>().IsRequired();
			builder.Property(x => x.ApproverMode).HasConversion<byte>().IsRequired();
			builder.Property(x => x.Status).HasConversion<byte>().IsRequired();

			builder.Property(x => x.ResolvedApproverCandidatesJson)
				.HasColumnName("ResolvedApproverCandidates")
				.HasColumnType("NVARCHAR(MAX)");

			builder.Property(x => x.HistoryJson).HasColumnType("NVARCHAR(MAX)");

			builder.HasOne(s => s.ApprovedByUser)
				.WithMany()
				.HasForeignKey(s => s.ApprovedBy)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(s => s.RejectedByUser)
				.WithMany()
				.HasForeignKey(s => s.RejectedBy)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasIndex(x => new { x.WorkflowInstanceId, x.Order })
				.IsUnique()
				.HasDatabaseName("UX_ASI_Workflow_Order");

			builder.HasIndex(x => new { x.Status, x.DueAt }).HasDatabaseName("IX_ASI_Status_DueAt");
			builder.HasIndex(x => x.SelectedApproverId).HasDatabaseName("IX_ASI_SelectedApprover");
			builder.HasIndex(x => new { x.ApprovedBy, x.ApprovedAt }).HasDatabaseName("IX_ASI_ApprovedBy_ApprovedAt");

			builder.ToTable(t =>
			{
				t.HasCheckConstraint("CK_ASI_ResolvedCandidates_JSON",
				    "([ResolvedApproverCandidates] IS NULL OR ISJSON([ResolvedApproverCandidates]) = 1)");
				t.HasCheckConstraint("CK_ASI_History_JSON",
				    "([HistoryJson] IS NULL OR ISJSON([HistoryJson]) = 1)");
			});
		}
	}
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	//public class ExpenseStepInstanceConfiguration : BaseEntityConfiguration<ExpenseStepInstance>
	//{
	//	public override void Configure(EntityTypeBuilder<ExpenseStepInstance> builder)
	//	{
	//		builder.ToTable("ExpenseStepInstances", "ExpenseWorkflow");
	//		builder.HasKey(x => x.Id);

	//		builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
	//		builder.Property(x => x.Order).IsRequired();
	//		builder.Property(x => x.FlowType).HasConversion<int>().IsRequired();
	//		builder.Property(x => x.ApproverMode).HasConversion<int>().IsRequired();
	//		builder.Property(x => x.Status).HasConversion<int>().IsRequired();
	//		builder.Property(x => x.SlaHours).IsRequired();
	//		builder.Property(x => x.ResolvedApproverCandidatesJson).HasColumnType("nvarchar(max)");
	//		builder.Property(x => x.Comments).HasColumnType("nvarchar(max)");
	//		builder.Property(x => x.HistoryJson).HasColumnType("nvarchar(max)");

	//		// Relationships
	//		builder.HasOne(x => x.WorkflowInstance)
	//			.WithMany(x => x.Steps)
	//			.HasForeignKey(x => x.WorkflowInstanceId)
	//			.OnDelete(DeleteBehavior.Cascade);

	//		builder.HasOne(x => x.ApprovedByUser)
	//			.WithMany()
	//			.HasForeignKey(x => x.ApprovedBy)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasOne(x => x.RejectedByUser)
	//		    .WithMany()
	//		    .HasForeignKey(x => x.RejectedBy)
	//		    .OnDelete(DeleteBehavior.Restrict);

	//		// Foreinkey to Step Template
	//		builder.HasOne<ExpenseStepTemplate>()
	//			.WithMany()
	//			.HasForeignKey(x => x.TemplateStepId)
	//			.OnDelete(DeleteBehavior.Restrict);
	//	}
	//}
}


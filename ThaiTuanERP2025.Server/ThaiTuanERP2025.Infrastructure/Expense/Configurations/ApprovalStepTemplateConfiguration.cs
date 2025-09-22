using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ApprovalStepTemplateConfiguration : IEntityTypeConfiguration<ApprovalStepTemplate>
	{
		public void Configure(EntityTypeBuilder<ApprovalStepTemplate> builder)
		{
			builder.ToTable("ApprovalStepTemplates", "Workflow");	
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Order).IsRequired();
			builder.Property(x => x.FlowType).HasConversion<byte>().IsRequired();
			builder.Property(x => x.SlaHours).IsRequired();
			builder.Property(x => x.ApproverMode).HasConversion<byte>().IsRequired();
			builder.Property(x => x.FixedApproverIdsJson)
				.HasColumnName("FixedApproverIds")
				.HasColumnType("NVARCHAR(MAX)");
			builder.Property(x => x.ResolverKey).HasMaxLength(100);
			builder.Property(x => x.ResolverParamsJson)
				.HasColumnName("ResolverParams")
				.HasColumnType("NVARCHAR(MAX)");
			builder.Property(x => x.AllowOverride).IsRequired().HasDefaultValue(false);
			builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

			// Quan hệ 1-n: Template → StepTemplate
			builder.HasOne(x => x.WorkflowTemplate)
				.WithMany(t => t.Steps)
				.HasForeignKey(x => x.WorkflowTemplateId)
				.OnDelete(DeleteBehavior.Cascade);
			// Unique (WorkflowTemplateId, Order)
			builder.HasIndex(x => new { x.WorkflowTemplateId, x.Order })
			       .IsUnique()
			       .HasDatabaseName("UX_AST_Workflow_Order");

			// Index phụ trợ (nếu cần lọc nhanh theo template/approver mode/flow type)
			builder.HasIndex(x => new { x.WorkflowTemplateId, x.ApproverMode });
			builder.HasIndex(x => new { x.WorkflowTemplateId, x.FlowType });

			// CHECK constraints (dùng API mới ToTable(...HasCheckConstraint...))
			builder.ToTable(t =>
			{
				// FlowType ∈ {0,1}
				t.HasCheckConstraint("CK_AST_FlowType", "[FlowType] IN (0,1)");

				// ApproverMode ∈ {0,1}
				t.HasCheckConstraint("CK_AST_ApproverMode", "[ApproverMode] IN (0,1)");

				// Nếu ApproverMode=Standard ⇒ FixedApproverIds phải là JSON hợp lệ (không NULL)
				t.HasCheckConstraint(
				    "CK_AST_FixedApprover_JSON",
				    "CASE WHEN [ApproverMode] = 0 THEN IIF([FixedApproverIds] IS NOT NULL AND ISJSON([FixedApproverIds]) = 1, 1, 0) ELSE 1 END = 1"
				);

				// Nếu ApproverMode=Condition ⇒ ResolverKey NOT NULL
				t.HasCheckConstraint(
				    "CK_AST_Resolver_Required_When_Condition",
				    "CASE WHEN [ApproverMode] = 1 THEN IIF([ResolverKey] IS NOT NULL, 1, 0) ELSE 1 END = 1"
				);

				// ResolverParams (nếu có) phải là JSON hợp lệ
				t.HasCheckConstraint(
				    "CK_AST_ResolverParams_JSON",
				    "([ResolverParams] IS NULL OR ISJSON([ResolverParams]) = 1)"
				);
			});

		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Expense.Configurations
{
	public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
	{
		public void Configure(EntityTypeBuilder<ApprovalStep> builder)
		{
			builder.ToTable("ApprovalSteps", "Expense");
			
			builder.Property(x => x.Title).IsRequired().HasMaxLength(256);
			builder.Property(x => x.Order).IsRequired();
			builder.Property(x => x.FlowType).IsRequired();
			builder.Property(x => x.SlaHours).HasDefaultValue(8);
			builder.Property(x => x.CandidateJson)
				.IsRequired()
				.HasColumnType("nvarchar(max)")
				.HasDefaultValue("[]");
			builder.Property(x => x.Description)
				.HasMaxLength(1000);
			
			// Thứ tự step là duy nhất trong 1 workflow
			builder.HasIndex(x => new { x.ApprovalWorkflowId, x.Order })
				.IsUnique();


		}
	}
}

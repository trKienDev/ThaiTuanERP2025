using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Infrastructure.Approval.Configurations
{
	public class ApprovalActionConfiguration : IEntityTypeConfiguration<ApprovalAction>
	{
		public void Configure(EntityTypeBuilder<ApprovalAction> builder)
		{
			builder.ToTable("Actions", "Approval");
			builder.HasKey(x => x.Id);	

			builder.Property(x => x.Action).HasConversion<int>().IsRequired();
			builder.Property(x => x.Comment).HasMaxLength(2000);
			builder.Property(x => x.AttachmentFileIdsJson).HasColumnType("nvarchar(max)");
			builder.Property(x => x.OccuredAt).IsRequired();

			builder.HasIndex(x => new { x.StepInstanceId, x.OccuredAt });
		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetApproverConfiguration : BaseEntityConfiguration<BudgetApprover>
	{
		public override void Configure(EntityTypeBuilder<BudgetApprover> builder)
		{
			builder.ToTable("BudgetApprover", "Finance");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.ApproverUserId).IsRequired();
			builder.HasOne(x => x.ApproverUser)
				.WithMany()
				.HasForeignKey(x => x.ApproverUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(x => x.IsActive).HasDefaultValue(true);
			builder.Property(x => x.SlaHours).IsRequired().HasDefaultValue(8);

			builder.HasIndex(x => x.ApproverUserId);
			builder.HasIndex(x => x.IsActive);

			ConfigureAuditUsers(builder);
		}
	}
}

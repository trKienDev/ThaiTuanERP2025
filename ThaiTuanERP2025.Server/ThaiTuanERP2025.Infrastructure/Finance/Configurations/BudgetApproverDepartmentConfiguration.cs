using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetApproverDepartmentConfiguration : IEntityTypeConfiguration<BudgetApproverDepartment>
	{
		public void Configure(EntityTypeBuilder<BudgetApproverDepartment> builder)
		{
			builder.ToTable("BudgetApproverDepartments", "Finance");

			builder.HasKey(x => new { x.BudgetApproverId, x.DepartmentId });

			builder.HasOne(x => x.BudgetApprover)
				.WithMany(a => a.Departments)
				.HasForeignKey(x => x.BudgetApproverId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Department)
				.WithMany()
				.HasForeignKey(x => x.DepartmentId)
				.OnDelete(DeleteBehavior.Restrict);
		} 
	}
}

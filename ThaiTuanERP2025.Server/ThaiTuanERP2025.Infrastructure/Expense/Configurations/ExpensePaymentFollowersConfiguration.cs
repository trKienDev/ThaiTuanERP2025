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
	public class ExpensePaymentFollowersConfiguration : IEntityTypeConfiguration<ExpensePaymentFollower>
	{
		public void Configure(EntityTypeBuilder<ExpensePaymentFollower> builder) {
			builder.ToTable("ExpensePaymentFollowers", "Expense");
			builder.HasKey(x => x.Id);

			builder.HasIndex(x => new { x.ExpensePaymentId, x.UserId }).IsUnique();

			builder.HasOne(x => x.ExpensePayment)
				.WithMany(p => p.Followers)
				.HasForeignKey(x => x.ExpensePaymentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}

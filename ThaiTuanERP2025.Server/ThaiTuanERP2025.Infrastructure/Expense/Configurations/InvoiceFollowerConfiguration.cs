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
	public class InvoiceFollowerConfiguration : IEntityTypeConfiguration<InvoiceFollwer>
	{
		public void Configure(EntityTypeBuilder<InvoiceFollwer> builder)
		{
			builder.ToTable("InvoiceFollowers", "Expense");
			builder.HasKey(f => new { f.InvoiceId, f.UserId});

			builder.HasOne(f => f.Invoice)
				.WithMany(i => i.Follwers)
				.HasForeignKey(f => f.InvoiceId);

			builder.HasOne(f => f.User)
				.WithMany()
				.HasForeignKey(f => f.UserId);
		}
	}
}

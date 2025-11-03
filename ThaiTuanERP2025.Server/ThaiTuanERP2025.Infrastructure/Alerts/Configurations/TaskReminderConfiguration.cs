using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Alerts.Entities;

namespace ThaiTuanERP2025.Infrastructure.Alerts.Configurations
{
	public sealed class TaskReminderConfiguration : IEntityTypeConfiguration<TaskReminder>
	{	
		public void Configure(EntityTypeBuilder<TaskReminder> builder) {
			builder.ToTable("TaskReminder", "Alerts");
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Title).HasMaxLength(256);
			builder.Property(x => x.Message).HasMaxLength(1024);

			builder.HasIndex(x => new { x.UserId, x.ResolvedAt, x.DueAt });
			builder.HasIndex(x => new { x.StepInstanceId, x.UserId }).IsUnique(false);
		}
	}
}

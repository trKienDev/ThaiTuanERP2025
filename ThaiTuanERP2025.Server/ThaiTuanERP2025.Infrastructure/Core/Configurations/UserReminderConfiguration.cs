using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public class UserReminderConfiguration : IEntityTypeConfiguration<UserReminder>
	{
		public void Configure(EntityTypeBuilder<UserReminder> builder) {
			builder.ToTable("UserReminders", "Core");

			builder.HasKey(r => r.Id);

			builder.Property(r => r.Subject).IsRequired().HasMaxLength(200);

			builder.Property(r => r.Message).IsRequired();

			builder.Property(r => r.LinkUrl).HasMaxLength(500);

			builder.Property(r => r.TriggerAt).IsRequired().HasColumnType("datetime2");

			builder.Property(r => r.IsTriggered).IsRequired();

			builder.Property(r => r.TriggeredAt).HasColumnType("datetime2");

			builder.HasOne(r => r.User)
				.WithMany()
				.HasForeignKey(r => r.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
